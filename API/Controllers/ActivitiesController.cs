using API.Controllers.Base;
using Application.Core;
using Application.UseCases.Activities.Commands;
using Application.UseCases.Activities.DTOs;
using Application.UseCases.Activities.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// API controller for managing Reactivities (Activities).
    /// </summary>
    public class ActivitiesController : BaseApiController
    {
        /// <summary>
        /// Retrieves a paginated list of activities.
        /// </summary>
        /// <param name="activityParams">The parameters for filtering and pagination.</param>
        /// <returns>A paged list of activities.</returns>
        [HttpGet]
        public async Task<ActionResult<PagedList<ActivityDto, DateTime?>>> GetActivities(
            [FromQuery] ActivityParams activityParams)
        {
            return HandleResult(await Mediator.Send(new GetActivityList(activityParams)));
        }

        /// <summary>
        /// Retrieves the details of a specific activity by its ID.
        /// </summary>
        /// <param name="id">The ID of the activity.</param>
        /// <returns>The details of the activity.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetActivityDetail([FromRoute] string id)
        {
            return HandleResult(await Mediator.Send(new GetActivityDetails.Query { Id = id }));
        }

        /// <summary>
        /// Creates a new activity.
        /// </summary>
        /// <param name="activityDto">The activity data.</param>
        /// <returns>The ID of the created activity.</returns>
        [HttpPost]
        public async Task<ActionResult<string>> CreateActivity([FromBody] CreateActivityDto activityDto)
        {
            return HandleResult((await Mediator.Send(new CreateActivity.Command { ActivityDto = activityDto })));
        }

        /// <summary>
        /// Updates an existing activity.
        /// </summary>
        /// <param name="id">The ID of the activity to update.</param>
        /// <param name="activity">The updated activity data.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<ActionResult> EditActivity(string id, [FromBody] EditActivityDto activity)
        {
            activity.Id = id;
            return HandleResult((await Mediator.Send(new EditActivity.Command { ActivityDto = activity })));
        }


        /// <summary>
        /// Deletes an activity.
        /// </summary>
        /// <param name="id">The ID of the activity to delete.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<ActionResult> DeleteActivity([FromRoute] string id)
        {
            return HandleResult((await Mediator.Send(new DeleteActivity.Command { Id = id })));
        }

        /// <summary>
        /// Toggles the attendance of the current user for a specific activity.
        /// </summary>
        /// <param name="id">The ID of the activity.</param>
        /// <returns>No content if successful.</returns>
        [HttpPost("{id}/attend")]
        public async Task<ActionResult> Attend([FromRoute] string id)
        {
            return HandleResult((await Mediator.Send(new UpdateAttendance.Command { Id = id })));
        }

    }
}
