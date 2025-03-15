﻿

using Application.Core;
using Domain.Entities;
using MediatR;
using Persistence.Data;

namespace Application.Activities.Queries
{
    public class GetActivityDetails
    {
        public class Query : IRequest<Result<Activity>>
        {
            public required string Id { get; set; }
        }
        public class Handler(AppDbContext context) : IRequestHandler<Query, Result<Activity>>
        {
            public async Task<Result<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await context.Activities.FindAsync([request.Id], cancellationToken);

                //if (activity == null) return Result<Activity>.Failure("Activity not found", 404);

                //return Result<Activity>.Success(activity); //Old

                return activity == null 
                    ? Result<Activity>.Failure("Activity not found", 404)
                    : Result<Activity>.Success(activity); //New
            }
        }
    }
}
