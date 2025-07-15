using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FullStackApp.API.Data;
using FullStackApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FullStackApp.API.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UsersController(AppDbContext db) { _db = db; }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
