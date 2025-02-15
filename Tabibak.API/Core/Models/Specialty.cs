using System.ComponentModel.DataAnnotations;

namespace Tabibak.API.Core.Models
{
    public class Specialty
    {
        [Key]
        public int SpecialtyId { get; set; }
        [MaxLength(100)]
        public string ArName { get; set; }
        [MaxLength(100)]
        public string EnName { get; set; }

        // ✅ Many-to-Many Relationship with Doctors
        public ICollection<DoctorSpecialty> DoctorSpecialties { get; set; } = new List<DoctorSpecialty>();
    }

}
