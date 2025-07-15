using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullStackApp.API.Models
{
    public class InfluencerProfile
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        // Public fields
        public string Name { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public string Bio { get; set; }
        public string OtherPublicFields { get; set; }
        // Private fields
        public string OtherPrivateFields { get; set; }
    }
}
