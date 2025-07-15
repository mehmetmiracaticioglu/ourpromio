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
    [Route("ads")]
    public class AdsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AdsController(AppDbContext db) { _db = db; }

        // GET /ads (published ads for influencers)
        [HttpGet]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> GetPublished()
        {
            var ads = await _db.Advertisements.Where(a => a.Status == AdvertisementStatus.PUBLISHED).ToListAsync();
            return Ok(ads);
        }

        // POST /ads/:id/apply (apply to ad)
        [HttpPost("{id}/apply")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> Apply(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var influencer = await _db.InfluencerProfiles.FirstOrDefaultAsync(i => i.UserId == userId);
            if (influencer == null) return Forbid();
            var exists = await _db.Applications.AnyAsync(a => a.AdId == id && a.InfluencerId == influencer.Id);
            if (exists) return BadRequest("Already applied");
            var app = new Application { AdId = id, InfluencerId = influencer.Id };
            _db.Applications.Add(app);
            await _db.SaveChangesAsync();
            return Ok(app);
        }

        // GET /ads/applications (influencer's own applications)
        [HttpGet("applications")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> GetOwnApplications()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var influencer = await _db.InfluencerProfiles.FirstOrDefaultAsync(i => i.UserId == userId);
            if (influencer == null) return Forbid();
            var apps = await _db.Applications.Include(a => a.Advertisement).Where(a => a.InfluencerId == influencer.Id).ToListAsync();
            return Ok(apps);
        }
    }
}
