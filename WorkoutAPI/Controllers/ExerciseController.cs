using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workout.Application.Common.Dto;
using Workout.Application.Services.Interface;
using Workout.Domain.Entities;
using WorkoutAPI.Common;
using Asp.Versioning;

namespace WorkoutAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/exercises")]
    [ApiController]
    [Authorize]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;
        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Exercise>>>> Get()
        {
            var response = await _exerciseService.GetAllExercises();
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<IEnumerable<Exercise>>.Ok(response.Values));
        }
    }
}
