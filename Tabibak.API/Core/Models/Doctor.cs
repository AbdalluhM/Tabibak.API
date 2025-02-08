using System.ComponentModel.DataAnnotations;
using Tabibak.Models;

namespace Tabibak.API.Core.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        public string Name => User.FullName;
        public string Qualification { get; set; }
        public string ContactInfo { get; set; }

        // ✅ Link to ApplicationUser (One-to-One)
        public string UserId { get; set; }
        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public ICollection<Specialty> Specialties { get; set; } = new List<Specialty>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
