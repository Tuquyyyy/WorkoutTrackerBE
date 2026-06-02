using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workout.Application.Common.Dto;
using Workout.Application.Services.Interface;
using WorkoutAPI.Common;
using Asp.Versioning;

namespace WorkoutAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/workout-schedules")]
    [ApiController]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleWorkoutService _scheduleWorkoutService;
        public ScheduleController(IScheduleWorkoutService scheduleWorkoutService)
        {
            _scheduleWorkoutService = scheduleWorkoutService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ScheduleWorkoutDto>>>> Get()
        {
            var response = await _scheduleWorkoutService.GetScheduleWorkoutsByUserId(User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<IEnumerable<ScheduleWorkoutDto>>.Ok(response.Values));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] ScheduleWorkoutDto scheduleWorkoutDto)
        {
            var response = await _scheduleWorkoutService.SetWorkoutSchedule(scheduleWorkoutDto, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<string>.Ok(response.Values));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Put(Guid id, [FromBody] ScheduleWorkoutDto scheduleWorkoutDto)
        {
            scheduleWorkoutDto.Id = id;
            var response = await _scheduleWorkoutService.UpdateScheduledWorkout(scheduleWorkoutDto, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<string>.Ok(response.Values));
        }

        [HttpDelete("{schedule_workout_id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(Guid schedule_workout_id)
        {
            var response = await _scheduleWorkoutService.DeleteScheduledWorkout(schedule_workout_id, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<string>.Ok(response.Values));
        }

        [HttpPut("{id:guid}/complete")]
        public async Task<ActionResult<ApiResponse<string>>> Complete(Guid id)
        {
            var response = await _scheduleWorkoutService.CompleteWorkout(id, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<string>.Ok(response.Values));
        }
    }
}
