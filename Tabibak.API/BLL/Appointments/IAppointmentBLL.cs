using Tabibak.Api.BLL.BaseReponse;
using Tabibak.API.Dtos.Appoinments;

namespace Tabibak.API.BLL.Appointments
{
    public interface IAppointmentBLL
    {
        Task<IResponse<AppointmentResponseDto>> CreateAppointmentAsync(CreateAppointmentDto dto);
        Task<IResponse<List<AppointmentResponseDto>>> GetAllAppointmentsAsync();
        Task<IResponse<AppointmentResponseDto>> GetAppointmentByIdAsync(int id);
        Task<IResponse<bool>> UpdateAppointmentAsync(int id, UpdateAppointmentDto dto);
        Task<IResponse<bool>> DeleteAppointmentAsync(int id);
        Task<IResponse<List<AppointmentResponseDto>>> GetAppointmentsByDoctorAsync(int doctorId);
        Task<IResponse<List<AppointmentResponseDto>>> GetAppointmentsByPatientAsync(int patientId);
    }
}
