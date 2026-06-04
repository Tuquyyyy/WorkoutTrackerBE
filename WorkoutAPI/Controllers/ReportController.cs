using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workout.Application.Common.Dto;
using Workout.Application.Services.Interface;
using WorkoutAPI.Common;
using Asp.Versioning;

namespace WorkoutAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/reports")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IWorkoutPlanService _workoutPlanService;
        public ReportController(IWorkoutPlanService workoutPlanService)
        {
            _workoutPlanService = workoutPlanService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<WorkoutReportDto>>> Get()
        {
            var response = await _workoutPlanService.GenerateReport(User);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<WorkoutReportDto>.Ok(response.Values));
        }
    }
}
