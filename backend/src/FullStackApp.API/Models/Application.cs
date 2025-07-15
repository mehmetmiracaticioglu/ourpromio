using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullStackApp.API.Models
{
    public enum ApplicationStatus { PENDING, APPROVED, REJECTED }
    public class Application
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Advertisement")]
        public int AdId { get; set; }
        public Advertisement Advertisement { get; set; }
        [ForeignKey("InfluencerProfile")]
        public int InfluencerId { get; set; }
        public InfluencerProfile InfluencerProfile { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.PENDING;
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DecisionAt { get; set; }
    }
}
