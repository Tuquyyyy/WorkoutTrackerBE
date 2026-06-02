using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workout.Application.Common.Dto;
using Workout.Application.Services.Interface;
using WorkoutAPI.Common;
using Asp.Versioning;

namespace WorkoutAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/workout-comments")]
    [ApiController]
    [Authorize]
    public class WorkoutCommentsController : ControllerBase
    {
        private readonly IWorkoutCommentsService _workoutCommentsService;
        public WorkoutCommentsController(IWorkoutCommentsService workoutCommentsService)
        {
            _workoutCommentsService = workoutCommentsService;
        }

        [HttpGet("{workout_id:guid}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkoutCommentsDto>>>> Get(Guid workout_id)
        {
            var response = await _workoutCommentsService.GetWorkoutCommentsByWorkoutId(workout_id, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<IEnumerable<WorkoutCommentsDto>>.Ok(response.Values));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] WorkoutCommentsDto workoutCommentsDto)
        {
            var response = await _workoutCommentsService.AddWorkoutComment(workoutCommentsDto, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<string>.Ok(response.Values));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Put(Guid id, [FromBody] WorkoutCommentsDto workoutCommentsDto)
        {
            workoutCommentsDto.Id = id;
            var response = await _workoutCommentsService.UpdateWorkoutComment(workoutCommentsDto, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<string>.Ok(response.Values));
        }

        [HttpDelete("{workout_comment_id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(Guid workout_comment_id)
        {
            var response = await _workoutCommentsService.DeleteWorkoutComment(workout_comment_id, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<string>.Ok(response.Values));
        }
    }
}
