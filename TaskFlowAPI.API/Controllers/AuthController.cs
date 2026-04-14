using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskFlowAPI.Application.Services;

namespace TaskFlowAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var success = await _authService.RegisterAsync(request.Username, request.Email, request.Password);
            if (!success) return BadRequest("Username or email already exists");
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.Username, request.Password);
            if (result == null) return Unauthorized("Invalid username or password");
            return Ok(new { result.Value.AccessToken, result.Value.RefreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);
            if (result == null) return Unauthorized("Invalid or expired refresh token");
            return Ok(new { result.Value.AccessToken, result.Value.RefreshToken });
        }
    }

    public record RegisterRequest(string Username, string Email, string Password);
    public record LoginRequest(string Username, string Password);
    public record RefreshRequest(string AccessToken, string RefreshToken);
}
