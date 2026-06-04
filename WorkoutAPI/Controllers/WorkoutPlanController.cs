using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workout.Application.Common.Dto;
using Workout.Application.Services.Interface;
using WorkoutAPI.Common;
using Asp.Versioning;

namespace WorkoutAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/workouts")]
    [ApiController]
    [Authorize]
    public class WorkoutPlanController : ControllerBase
    {
        private readonly IWorkoutPlanService _workoutPlanService;
        public WorkoutPlanController(IWorkoutPlanService workoutPlanService)
        {
            _workoutPlanService = workoutPlanService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkoutPlanResponseDto>>>> Get()
        {
            var response = await _workoutPlanService.GetWorkoutsbyUserId(User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<IEnumerable<WorkoutPlanResponseDto>>.Ok(response.Values));
        }

        [HttpGet("{workout_plan_id:Guid}")]
        public async Task<ActionResult<ApiResponse<WorkoutPlanDto>>> Get(Guid workout_plan_id)
        {
            var response = await _workoutPlanService.GetByWorkouPlanId(workout_plan_id, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<WorkoutPlanDto>.Ok(response.Values));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<WorkoutPlanDto>>> Post([FromBody] WorkoutPlanDto workoutPlanDto)
        {
            var response = await _workoutPlanService.AddWorkoutPlan(workoutPlanDto, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<WorkoutPlanDto>.Ok(response.Values));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Put(Guid id, [FromBody] WorkoutPlanDto workoutPlanDto)
        {
            workoutPlanDto.Id = id;
            var response = await _workoutPlanService.UpdateWorkoutPlan(workoutPlanDto, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<string>.Ok(response.Values));
        }

        [HttpDelete("{workout_plan_id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(Guid workout_plan_id)
        {
            var response = await _workoutPlanService.DeleteWorkoutPlan(workout_plan_id, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<string>.Ok(response.Values));
        }
    }
}
