using System.ComponentModel.DataAnnotations;
using Tabibak.Models;

namespace Tabibak.API.Core.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        public string Name => User.FullName;
        public string? Qualification { get; set; }
        public string? ContactInfo { get; set; }
        public string? Description { get; set; }
        public decimal? Fees { get; set; }

        // ✅ Link to ApplicationUser (One-to-One)
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        // ✅ Many-to-Many Relationship with Specialties (Fixed)
        public ICollection<DoctorSpecialty> DoctorSpecialties { get; set; } = new List<DoctorSpecialty>();

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }


}
