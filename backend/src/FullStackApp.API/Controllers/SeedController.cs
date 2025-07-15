
using Microsoft.AspNetCore.Mvc;
using FullStackApp.API.Data;
using FullStackApp.API.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FullStackApp.API.Controllers
{
    [ApiController]
    [Route("seed")]
    public class SeedController : ControllerBase
    {
        private readonly AppDbContext _db;
        public SeedController(AppDbContext db) { _db = db; }

        [HttpPost]
        public async Task<IActionResult> Seed()
        {
            if (await _db.Users.AnyAsync()) return BadRequest("Already seeded");
            var admin = new User { Email = "admin@site.com", PasswordHash = "admin", Role = UserRole.ADMIN };
            var companyUser = new User { Email = "company@site.com", PasswordHash = "company", Role = UserRole.COMPANY };
            var influencerUser = new User { Email = "influencer@site.com", PasswordHash = "influencer", Role = UserRole.INFLUENCER };
            _db.Users.AddRange(admin, companyUser, influencerUser);
            await _db.SaveChangesAsync();
            var company = new Company { UserId = companyUser.Id, CompanyName = "TestCo", ContactInfo = "test@co.com" };
            var influencer = new InfluencerProfile { UserId = influencerUser.Id, Name = "Test Influencer", Age = 25, Location = "Istanbul", Bio = "Bio", OtherPublicFields = "", OtherPrivateFields = "Secret" };
            _db.Companies.Add(company);
            _db.InfluencerProfiles.Add(influencer);
            await _db.SaveChangesAsync();
            return Ok("Seeded");
        }
    }
}
