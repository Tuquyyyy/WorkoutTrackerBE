using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workout.Application.Common.Dto;
using Workout.Application.Services.Interface;
using WorkoutAPI.Common;
using Asp.Versioning;

namespace WorkoutAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/workout-exercises")]
    [ApiController]
    [Authorize]
    public class WorkoutExerciseController : ControllerBase
    {
        private readonly IWorkoutExerciseService _workoutExerciseService;
        public WorkoutExerciseController(IWorkoutExerciseService workoutExerciseService)
        {
            _workoutExerciseService = workoutExerciseService;
        }

        [HttpGet("{workout_plan_id:Guid}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkoutExerciseDto>>>> Get(Guid workout_plan_id)
        {
            var response = await _workoutExerciseService.GetWorkoutExerciseById(workout_plan_id, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<IEnumerable<WorkoutExerciseDto>>.Ok(response.Values));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] WorkoutExerciseDto model)
        {
            var response = await _workoutExerciseService.AddWorkoutExercise(model, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<string>.Ok(response.Values));
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Put(Guid id, [FromBody] WorkoutExerciseDto model)
        {
            model.Id = id;
            var response = await _workoutExerciseService.UpdateWorkoutExercise(model, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<string>.Ok(response.Values));
        }

        [HttpDelete("{workout_exercise_id:Guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(Guid workout_exercise_id)
        {
            var response = await _workoutExerciseService.DeleteWorkoutExercise(workout_exercise_id, User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<string>.Ok(response.Values));
        }
    }
}
