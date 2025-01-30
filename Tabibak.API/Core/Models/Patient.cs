using System.ComponentModel.DataAnnotations;

namespace Tabibak.API.Core.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Navigation properties
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Doctor> Wishlist { get; set; } = new List<Doctor>();
    }
}
