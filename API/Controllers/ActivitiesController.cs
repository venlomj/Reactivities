using API.Controllers.Base;
using Application.Activities.Commands;
using Application.Activities.DTOs;
using Application.Activities.Queries;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await Mediator.Send(new GetActivityList.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivityDetail([FromRoute] string id)
        {
            return HandleActionResult(await Mediator.Send(new GetActivityDetails.Query { Id = id }));
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateActivity([FromBody] CreateActivityDto activityDto)
        {
            return HandleActionResult((await Mediator.Send(new CreateActivity.Command { ActivityDto = activityDto })));
        }

        [HttpPut]
        public async Task<ActionResult> EditActivity([FromBody] EditActivityDto activity)
        {
            return HandleActionResult((await Mediator.Send(new EditActivity.Command { ActivityDto = activity })));
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<Activity>> DeleteActivity([FromRoute] string id)
        {
            return HandleActionResult((await Mediator.Send(new DeleteActivity.Command { Id = id })));
        }
    }
}
