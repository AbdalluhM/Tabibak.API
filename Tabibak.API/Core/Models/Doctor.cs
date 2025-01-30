using System.ComponentModel.DataAnnotations;

namespace Tabibak.API.Core.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Qualification { get; set; }
        public string ContactInfo { get; set; }

        // Navigation properties
        public ICollection<Specialty> Specialties { get; set; } = new List<Specialty>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
