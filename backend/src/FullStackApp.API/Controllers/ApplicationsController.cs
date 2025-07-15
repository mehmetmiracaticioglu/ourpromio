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
    [Route("applications")]
    public class ApplicationsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ApplicationsController(AppDbContext db) { _db = db; }

        // PUT /applications/:id (approve/reject by company)
        [HttpPut("{id}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest req)
        {
            var app = await _db.Applications.Include(a => a.Advertisement).FirstOrDefaultAsync(a => a.Id == id);
            if (app == null) return NotFound();
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var company = await _db.Companies.FirstOrDefaultAsync(c => c.UserId == userId);
            if (company == null || app.Advertisement.CompanyId != company.Id) return Forbid();
            app.Status = req.Status;
            app.DecisionAt = System.DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        public class UpdateStatusRequest
        {
            public ApplicationStatus Status { get; set; }
        }
    }
}
