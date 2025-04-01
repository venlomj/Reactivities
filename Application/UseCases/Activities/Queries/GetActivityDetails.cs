using Application.Core;
using Application.Interfaces;
using Application.UseCases.Activities.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.UseCases.Activities.Queries
{
    public class GetActivityDetails
    {
        public class Query : IRequest<Result<ActivityDto>>
        {
            public required string Id { get; set; }
        }
        public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Query, Result<ActivityDto>>
        {
            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await context.Activities
                    .ProjectTo<ActivityDto>(mapper.ConfigurationProvider,
                        new { currentUserId = userAccessor.GetUserId() })
                    .FirstOrDefaultAsync(x => request.Id == x.Id, cancellationToken);

                //if (activity == null) return Result<Activity>.Failure("Activity not found", 404);

                //return Result<Activity>.Success(activity); //Old

                return activity == null
                    ? Result<ActivityDto>.Failure("Activity not found", 404)
                    : Result<ActivityDto>.Success(activity); //New
            }
        }
    }
}
