using Microsoft.EntityFrameworkCore;
using Tabibak.Api.BLL.BaseReponse;
using Tabibak.Api.BLL.Constants;
using Tabibak.API.Core.Models;
using Tabibak.API.Dtos.Appoinments;
using Tabibak.Context;

namespace Tabibak.API.BLL.Appointments
{
    public class AppointmentBLL : IAppointmentBLL
    {
        private readonly ApplicationDbcontext _context;

        public AppointmentBLL(ApplicationDbcontext context)
        {
            _context = context;
        }

        // Create an appointment
        public async Task<IResponse<AppointmentResponseDto>> CreateAppointmentAsync(CreateAppointmentDto dto)
        {
            var response = new Response<AppointmentResponseDto>();
            var doctor = await _context.Doctors.FindAsync(dto.DoctorId);
            var patient = await _context.Patients.FindAsync(dto.PatientId);

            if (doctor == null || patient == null)
            {
                throw new ArgumentException("Doctor or Patient not found.");
            }

            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                AppointmentDate = dto.AppointmentDate
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return response.CreateResponse(new AppointmentResponseDto
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = appointment.DoctorId,
                DoctorName = doctor.Name,
                PatientId = appointment.PatientId,
                PatientName = patient.Name,
                AppointmentDate = appointment.AppointmentDate
            });
        }

        // Get all appointments
        public async Task<IResponse<List<AppointmentResponseDto>>> GetAllAppointmentsAsync()
        {
            var response = new Response<List<AppointmentResponseDto>>();
            return response.CreateResponse(await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Select(a => new AppointmentResponseDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.Name,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.Name,
                    AppointmentDate = a.AppointmentDate
                })
                .ToListAsync());
        }

        // Get appointment by ID
        public async Task<IResponse<AppointmentResponseDto>> GetAppointmentByIdAsync(int id)
        {
            var response = new Response<AppointmentResponseDto>();

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
                return response.CreateResponse(MessageCodes.NotFound, nameof(appointment));

            return response.CreateResponse(new AppointmentResponseDto
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor.Name,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient.Name,
                AppointmentDate = appointment.AppointmentDate
            });
        }

        // Update an appointment date
        public async Task<IResponse<bool>> UpdateAppointmentAsync(int id, UpdateAppointmentDto dto)
        {
            var response = new Response<bool>();
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return response.CreateResponse(false);

            appointment.AppointmentDate = dto.AppointmentDate;
            await _context.SaveChangesAsync();
            return response.CreateResponse(true);
        }

        // Delete an appointment
        public async Task<IResponse<bool>> DeleteAppointmentAsync(int id)
        {
            var response = new Response<bool>();
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return response.CreateResponse(false);

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return response.CreateResponse(true);
        }

        // Get appointments by doctor
        public async Task<IResponse<List<AppointmentResponseDto>>> GetAppointmentsByDoctorAsync(int doctorId)
        {
            var response = new Response<List<AppointmentResponseDto>>();
            return response.CreateResponse(await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Patient)
                .Select(a => new AppointmentResponseDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.Name,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.Name,
                    AppointmentDate = a.AppointmentDate
                })
                .ToListAsync());
        }

        // Get appointments by patient
        public async Task<IResponse<List<AppointmentResponseDto>>> GetAppointmentsByPatientAsync(int patientId)
        {
            var response = new Response<List<AppointmentResponseDto>>();
            return response.CreateResponse(await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Doctor)
                .Select(a => new AppointmentResponseDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.Name,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.Name,
                    AppointmentDate = a.AppointmentDate
                })
                .ToListAsync());
        }
    }
}
