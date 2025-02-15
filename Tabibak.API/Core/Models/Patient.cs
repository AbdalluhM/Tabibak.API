using System.ComponentModel.DataAnnotations;
using Tabibak.Models;

namespace Tabibak.API.Core.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        // ✅ Link to ApplicationUser (One-to-One)
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }

}
