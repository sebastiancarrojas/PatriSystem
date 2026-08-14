using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatriSystem.API.DTOs.Request;
using PatriSystem.Domain.Interfaces.Services;

namespace PatriSystem.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var response = await _authService.LoginAsync(dto.Email, dto.Password);
            if (!response.IsSuccess)
                return Unauthorized(response);

            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
        {
            var response = await _authService.ForgotPasswordAsync(dto.Email);
            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
        {
            var response = await _authService.ResetPasswordAsync(dto.Email, dto.Token, dto.NewPassword);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}