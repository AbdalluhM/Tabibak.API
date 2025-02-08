using System.ComponentModel.DataAnnotations;
using Tabibak.Models;

namespace Tabibak.API.Core.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        [MaxLength(100)]
        public string Name => User.FullName;
        [MaxLength(100)]
        // ✅ Link to ApplicationUser (One-to-One)
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        // Navigation properties
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Doctor> Wishlist { get; set; } = new List<Doctor>();
    }
}
