namespace Tabibak.API.Dtos.Appoinments
{
    public class AppointMent
    {

    }
    public class CreateAppointmentDto
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }

    public class UpdateAppointmentDto
    {
        public DateTime AppointmentDate { get; set; }
    }

    public class AppointmentResponseDto
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime AppointmentDate { get; set; }
    }

}
