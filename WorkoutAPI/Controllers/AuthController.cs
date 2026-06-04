using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workout.Application.Common.Dto;
using Workout.Application.Common.Result;
using Workout.Application.Services.Implementation;
using Workout.Application.Services.Interface;
using WorkoutAPI.Common;
using Asp.Versioning;

namespace WorkoutAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto model)
        {
            var response = await _authService.Login(model);

            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<LoginResponseDto>.Ok(response.Values));
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<string>>> Register([FromBody] RegisterRequestDto model)
        {
            var response = await _authService.Register(model);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<string>.Ok(response.Values));
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var userIdResult = _authService.GetUserId(User);
            if (userIdResult.IsFailure)
            {
                return Unauthorized(ApiResponse<object>.Fail(userIdResult.Error.message));
            }

            var response = await _authService.ChangePassword(userIdResult.Values, model);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<string>.Ok(response.Values));
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> UpdateProfile([FromBody] UpdateProfileDto model)
        {
            var userIdResult = _authService.GetUserId(User);
            if (userIdResult.IsFailure)
            {
                return Unauthorized(ApiResponse<object>.Fail(userIdResult.Error.message));
            }

            var response = await _authService.UpdateProfile(userIdResult.Values, model);
            if (response.IsFailure)
            {
                return BadRequest(ApiResponse<object>.Fail(response.Error.message));
            }
            return Ok(ApiResponse<LoginResponseDto>.Ok(response.Values));
        }
    }
}
