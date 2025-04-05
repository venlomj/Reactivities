using API.Controllers.Base;
using Application.Core;
using Application.UseCases.Activities.Commands;
using Application.UseCases.Activities.DTOs;
using Application.UseCases.Activities.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<PagedList<ActivityDto, DateTime?>>> GetActivities(
            [FromQuery] ActivityParams activityParams)
        {
            return HandleResult(await Mediator.Send(new GetActivityList(activityParams)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetActivityDetail([FromRoute] string id)
        {
            return HandleResult(await Mediator.Send(new GetActivityDetails.Query { Id = id }));
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateActivity([FromBody] CreateActivityDto activityDto)
        {
            return HandleResult((await Mediator.Send(new CreateActivity.Command { ActivityDto = activityDto })));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<ActionResult> EditActivity(string id, [FromBody] EditActivityDto activity)
        {
            activity.Id = id;
            return HandleResult((await Mediator.Send(new EditActivity.Command { ActivityDto = activity })));
        }



        [HttpDelete("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<ActionResult> DeleteActivity([FromRoute] string id)
        {
            return HandleResult((await Mediator.Send(new DeleteActivity.Command { Id = id })));
        }

        [HttpPost("{id}/attend")]
        public async Task<ActionResult> Attend([FromRoute] string id)
        {
            return HandleResult((await Mediator.Send(new UpdateAttendance.Command { Id = id })));
        }

    }
}
