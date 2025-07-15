using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FullStackApp.API.Services;
using FullStackApp.API.Models;
using System.Threading.Tasks;

namespace FullStackApp.API.Controllers
{
    [ApiController]
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPut("users/{id}/role")]
        public async Task<IActionResult> ChangeUserRole(int id, [FromBody] ChangeRoleRequest req)
        {
            var result = await _userService.ChangeUserRoleAsync(id, req.NewRole);
            if (!result) return NotFound();
            return NoContent();
        }
    }

    public class ChangeRoleRequest
    {
        public UserRole NewRole { get; set; }
    }
}
