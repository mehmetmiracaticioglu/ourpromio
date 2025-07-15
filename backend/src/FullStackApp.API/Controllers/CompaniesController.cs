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
    [Route("companies")]
    [Authorize(Roles = "Company")]
    public class CompaniesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CompaniesController(AppDbContext db) { _db = db; }

        // POST /ads
        [HttpPost("ads")]
        public async Task<IActionResult> CreateAd([FromBody] Advertisement ad)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var company = await _db.Companies.FirstOrDefaultAsync(c => c.UserId == userId);
            if (company == null) return Forbid();
            ad.CompanyId = company.Id;
            _db.Advertisements.Add(ad);
            await _db.SaveChangesAsync();
            return Ok(ad);
        }

        // GET /ads (own ads)
        [HttpGet("ads")]
        public async Task<IActionResult> GetOwnAds()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var company = await _db.Companies.FirstOrDefaultAsync(c => c.UserId == userId);
            if (company == null) return Forbid();
            var ads = await _db.Advertisements.Where(a => a.CompanyId == company.Id).ToListAsync();
            return Ok(ads);
        }

        // GET /ads/:id/applications
        [HttpGet("ads/{adId}/applications")]
        public async Task<IActionResult> GetApplications(int adId)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var company = await _db.Companies.FirstOrDefaultAsync(c => c.UserId == userId);
            if (company == null) return Forbid();
            var ad = await _db.Advertisements.FirstOrDefaultAsync(a => a.Id == adId && a.CompanyId == company.Id);
            if (ad == null) return Forbid();
            var apps = await _db.Applications.Include(a => a.InfluencerProfile).Where(a => a.AdId == adId).ToListAsync();
            // Only public influencer fields
            var result = apps.Select(a => new {
                a.Id, a.Status, a.AppliedAt, a.DecisionAt,
                Influencer = new {
                    a.InfluencerProfile.Id,
                    a.InfluencerProfile.Name,
                    a.InfluencerProfile.Age,
                    a.InfluencerProfile.Location,
                    a.InfluencerProfile.Bio,
                    a.InfluencerProfile.OtherPublicFields
                }
            });
            return Ok(result);
        }
    }
}
