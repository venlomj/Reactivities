using Application.Core;
using Application.Interfaces;
using Application.UseCases.Activities.DTOs;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Persistence.Data;

namespace Application.UseCases.Activities.Commands
{
    public class CreateActivity
    {
        public class Command : IRequest<Result<string>>
        {
            public required CreateActivityDto ActivityDto { get; set; }
        }
        public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor)
            : IRequestHandler<Command, Result<string>>
        {
            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await userAccessor.GetUserAsync();

                var activity = mapper.Map<Activity>(request.ActivityDto);

                context.Activities.Add(activity);

                var attendee = new ActivityAttendee
                {
                    ActivityId = activity.Id,
                    UserId = user.Id,
                    IsHost = true
                };

                activity.Attendees.Add(attendee);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                return !result
                    ? Result<string>.Failure("Failed to create the activity.", 400)
                    : Result<string>.Success(activity.Id);
            }
        }
    }
}
