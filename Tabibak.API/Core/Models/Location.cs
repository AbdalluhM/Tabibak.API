using System.ComponentModel.DataAnnotations;

namespace Tabibak.API.Core.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } // e.g., "Cairo", "Alexandria"

        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>(); // One-to-Many Relationship
    }

}
