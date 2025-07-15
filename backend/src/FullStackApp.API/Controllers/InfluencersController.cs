using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FullStackApp.API.Data;
using FullStackApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackApp.API.Controllers
{
    [ApiController]
    [Route("influencers")]
    public class InfluencersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public InfluencersController(AppDbContext db) { _db = db; }

        // GET /influencers (public fields only)
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.InfluencerProfiles
                .Select(x => new {
                    x.Id, x.Name, x.Age, x.Location, x.Bio, x.OtherPublicFields
                }).ToListAsync();
            return Ok(list);
        }

        // GET /influencers/:id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var profile = await _db.InfluencerProfiles.FindAsync(id);
            if (profile == null) return NotFound();
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (profile.UserId == userId || User.IsInRole("Admin"))
                return Ok(profile); // all fields
            return Ok(new {
                profile.Id, profile.Name, profile.Age, profile.Location, profile.Bio, profile.OtherPublicFields
            });
        }

        // PUT /influencers/:id (only self)
        [HttpPut("{id}")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> Update(int id, [FromBody] InfluencerProfile update)
        {
            var profile = await _db.InfluencerProfiles.FindAsync(id);
            if (profile == null) return NotFound();
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (profile.UserId != userId) return Forbid();
            profile.Name = update.Name;
            profile.Age = update.Age;
            profile.Location = update.Location;
            profile.Bio = update.Bio;
            profile.OtherPublicFields = update.OtherPublicFields;
            profile.OtherPrivateFields = update.OtherPrivateFields;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
