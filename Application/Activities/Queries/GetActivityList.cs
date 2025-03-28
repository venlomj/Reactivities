using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Activities.Queries
{
    public class GetActivityList
    {

        // Defines the Query request object, implementing IRequest from MediatR.
        // It returns a paginated list of ActivityDto objects with a DateTime? cursor for pagination.
        public class Query : IRequest<Result<PagedList<ActivityDto, DateTime?>>>
        {
            // Required property to hold the query parameters (e.g., filters, page size).
            public required ActivityParams Params { get; set; }
        }

        // Handler class processes the query. Implements IRequestHandler to handle the Query request.
        public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor)
            : IRequestHandler<Query, Result<PagedList<ActivityDto, DateTime?>>>
        {
            // Handles the logic for retrieving and returning the activity list.
            public async Task<Result<PagedList<ActivityDto, DateTime?>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Step 1: Create a base query, ordered by activity date.
                // Filters activities to only include those with a date >= the provided cursor or start date.
                var query = context.Activities
                    .OrderBy(x => x.Date) // Order activities by date in ascending order.
                    .Where(x => x.Date >= (request.Params.Cursor ?? request.Params.StartDate)) // Apply cursor-based pagination.
                    .AsQueryable(); // Enables further query modification.

                // Step 2: Apply filters based on the request parameters.
                if (!string.IsNullOrEmpty(request.Params.Filter))
                {
                    query = request.Params.Filter switch
                    {
                        // Filter for activities the current user is attending.
                        "isGoing" => query.Where(x =>
                            x.Attendees.Any(a => a.UserId == userAccessor.GetUserId())),

                        // Filter for activities the current user is hosting.
                        "isHost" => query.Where(x =>
                            x.Attendees.Any(a => a.IsHost && a.UserId == userAccessor.GetUserId())),

                        // Default: No additional filtering.
                        _ => query
                    };
                }

                // Step 3: Project the query results into ActivityDto objects.
                var projectedActivities = query.ProjectTo<ActivityDto>(mapper.ConfigurationProvider,
                    new { currentUserId = userAccessor.GetUserId() });

                // Step 4: Apply pagination by limiting the result set to page size + 1.
                var activities = await projectedActivities
                    .Take(request.Params.PageSize + 1) // Fetch one extra item to determine if there are more pages.
                    .ToListAsync(cancellationToken); // Execute the query asynchronously.

                // Step 5: Determine the next cursor for pagination.
                DateTime? nextCursor = null; // Initialize next cursor as null.
                if (activities.Count > request.Params.PageSize)
                {
                    nextCursor = activities.Last().Date; // Set the next cursor to the last item's date.
                    activities.RemoveAt(activities.Count - 1); // Remove the extra item to return the correct page size.
                }

                // Step 6: Return the paginated result wrapped in a success response.
                return Result<PagedList<ActivityDto, DateTime?>>.Success(
                    new PagedList<ActivityDto, DateTime?>
                    {
                        Items = activities, // The current page of activities.
                        NextCursor = nextCursor // The cursor for the next page (if any).
                    });
            }
        }
    }
}
