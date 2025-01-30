using System.ComponentModel.DataAnnotations;

namespace Tabibak.API.Core.Models
{
    public class Specialty
    {
        [Key]
        public int SpecialtyId { get; set; }
        public string Name { get; set; }

        // Navigation properties
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
