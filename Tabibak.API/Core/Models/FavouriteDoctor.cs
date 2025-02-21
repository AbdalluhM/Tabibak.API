using System.ComponentModel.DataAnnotations;

namespace Tabibak.API.Core.Models
{
    public class FavoriteDoctor
    {
        [Key]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }

}
