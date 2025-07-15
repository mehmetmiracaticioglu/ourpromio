using Microsoft.AspNetCore.Mvc;
using FullStackApp.API.Services;
using FullStackApp.API.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FullStackApp.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        public AuthController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var user = await _userService.RegisterAsync(req.Email, req.Password, req.Role);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _userService.AuthenticateAsync(req.Email, req.Password);
            if (user == null) return Unauthorized();
            var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role.ToString());
            return Ok(new { token });
        }
    }

    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
