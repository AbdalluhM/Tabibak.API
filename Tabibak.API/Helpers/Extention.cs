using Tabibak.API.Core.Models;

namespace Tabibak.API.Helpers
{
    public static class Extention
    {
        public static string ToQRCodeContent(this Appointment appointment)
        {

            return $"BEGIN:VEVENT\n" +
               $"DoctorName:{appointment.Doctor.User.FullName}\n" +
               $"DTSTART:{appointment.AppointmentDate:yyyyMMddTHHmmss}\n" +
               $"LOCATION:{appointment.Doctor.Location.Name}\n" +
               $"PatientName:{appointment.Patient.User.FullName}\n" +
               $"END:VEVENT";
        }
    }
}
