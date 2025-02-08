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

        // Navigation properties
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
