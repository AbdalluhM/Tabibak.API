using Microsoft.EntityFrameworkCore;
using Tabibak.Api.BLL.BaseReponse;
using Tabibak.Api.BLL.Constants;
using Tabibak.API.Core.Models;
using Tabibak.API.Dtos.Appoinments;
using Tabibak.API.Helpers.Enums;
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
        public async Task<IResponse<AppointmentResponseDto>> CreateAvailableAppointmentAsync(CreateAppointmentDto dto)
        {
            var response = new Response<AppointmentResponseDto>();

            var doctor = await _context.Doctors.Include(d => d.User).FirstOrDefaultAsync(d => d.DoctorId == dto.DoctorId);
            if (doctor == null)
            {
                return response.CreateResponse(MessageCodes.NotFound, nameof(doctor));
            }

            // ✅ Ensure no duplicate slots
            bool slotExists = await _context.Appointments
                .AnyAsync(a => a.DoctorId == dto.DoctorId && a.AppointmentDate == dto.AppointmentDate);

            if (slotExists)
            {
                return response.CreateResponse(MessageCodes.AlreadyExists, nameof(AppointMent));
            }

            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                Status = AppointmentStatus.Available,
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return response.CreateResponse(new AppointmentResponseDto
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                DoctorName = doctor.Name,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status
            });
        }
        public async Task<IResponse<int>> BookAppointmentAsync(int appointmentId, int patientId)
        {
            var response = new Response<int>();

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId && a.Status == AppointmentStatus.Available);

            if (appointment == null)
            {
                return response.CreateResponse(MessageCodes.NotFound, nameof(appointment));
            }

            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null)
            {
                return response.CreateResponse(MessageCodes.NotFound, nameof(patient));
            }

            // ✅ Book the appointment
            appointment.PatientId = patientId;
            appointment.Status = AppointmentStatus.Booked;

            await _context.SaveChangesAsync();

            return response.CreateResponse(appointmentId);
        }

        public async Task<IResponse<AppointmentResponseDto>> CreateAppointmentAsync(CreateAppointmentDto dto)
        {

            var response = new Response<AppointmentResponseDto>();
            var doctor = await _context.Doctors.FindAsync(dto.DoctorId);
            var patient = await _context.Patients.FindAsync(dto.PatientId);

            if (doctor == null || patient == null)
            {
                return response.CreateResponse(MessageCodes.NotFound, $"{nameof(doctor)}or{nameof(patient)}");
            }

            // ✅ Check if doctor is available at the requested time
            bool isDoctorBusy = await _context.Appointments
                .AnyAsync(a => a.DoctorId == dto.DoctorId && a.AppointmentDate == dto.AppointmentDate);

            if (isDoctorBusy)
            {
                return response.CreateResponse(MessageCodes.AlreadyExists, nameof(Appointment));
            }

            // ✅ Create a new appointment
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
                PatientName = patient.User.FullName,
                AppointmentDate = appointment.AppointmentDate
            });
        }

        // Get all appointments
        public async Task<IResponse<List<AppointmentResponseDto>>> GetAvailableAppointmentsAsync(int doctorId)
        {
            var response = new Response<List<AppointmentResponseDto>>();

            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Available)
                .Select(a => new AppointmentResponseDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.Name,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status
                })
                .ToListAsync();

            return response.CreateResponse(appointments);
        }

        public async Task<IResponse<List<AppointmentResponseDto>>> GetAllAppointmentsAsync()
        {
            var response = new Response<List<AppointmentResponseDto>>();
            return response.CreateResponse(await _context.Appointments
                 .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .Include(a => a.Patient)
                .ThenInclude(a => a.User)
                .Select(a => new AppointmentResponseDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.Name,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.User.FullName,
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
                .ThenInclude(d => d.User)
                .Include(a => a.Patient)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
                return response.CreateResponse(MessageCodes.NotFound, nameof(appointment));

            return response.CreateResponse(new AppointmentResponseDto
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor.Name,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient.User.FullName,
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
                 .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .Include(a => a.Patient)
                .ThenInclude(a => a.User)
                .Select(a => new AppointmentResponseDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.User.FullName,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.User.FullName,
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
                .ThenInclude(d => d.User)
                .Include(a => a.Patient)
                .ThenInclude(a => a.User)
                .Select(a => new AppointmentResponseDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.Name,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.User.FullName,
                    AppointmentDate = a.AppointmentDate
                })
                .ToListAsync());
        }
    }
}
