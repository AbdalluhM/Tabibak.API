using System.ComponentModel.DataAnnotations;
using Tabibak.API.Helpers.Enums;

namespace Tabibak.API.Core.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int? PatientId { get; set; }
        public Patient? Patient { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Available;
    }


}
