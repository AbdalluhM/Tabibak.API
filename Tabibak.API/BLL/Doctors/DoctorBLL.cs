using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tabibak.Api.BLL.BaseReponse;
using Tabibak.Api.BLL.Constants;
using Tabibak.API.Core.Models;
using Tabibak.API.Dtos.Specialities;
using Tabibak.API.Helpers.Enums;
using Tabibak.Context;

namespace Tabibak.API.BLL.Doctors
{
    public class DoctorBLL : IDoctorBLL
    {
        private readonly ApplicationDbcontext _dbcontext;
        private readonly IMapper _mapper;

        public DoctorBLL(IMapper mapper, ApplicationDbcontext dbcontext)
        {
            _mapper = mapper;
            _dbcontext = dbcontext;
        }
        public async Task<IResponse<List<GetDoctoctorBySpecialtyDto>>> GetDoctorsBySpecialtyAsync(int specialtyId)
        {
            var response = new Response<List<GetDoctoctorBySpecialtyDto>>();
            try
            {
                var doctors = await _dbcontext.Doctors
                    .Where(d => d.DoctorSpecialties.Any(s => s.SpecialtyId == specialtyId))  // ✅ FIXED QUERY
                    .Include(d => d.User)  // ✅ Ensure ApplicationUser data is loaded
                    .ToListAsync();

                return response.CreateResponse(_mapper.Map<List<GetDoctoctorBySpecialtyDto>>(doctors));
            }
            catch (Exception e)
            {
                throw;  // Keep for debugging
            }
        }
        public async Task<IResponse<DoctorDetailResultDto>> GetDoctorDetailsAsync(int doctorId)
        {
            var response = new Response<DoctorDetailResultDto>();

            var doctor = await _dbcontext.Doctors
                .Where(d => d.DoctorId == doctorId)
                .Include(d => d.User)
                .Include(d => d.Reviews)
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync();

            if (doctor == null)
            {
                return response.CreateResponse(MessageCodes.NotFound, nameof(doctor));
            }

            // ✅ Calculate the average wait time in memory
            var waitedTime = CalculateWaitedTime(doctor.Appointments);

            var result = new DoctorDetailResultDto
            {
                Id = doctor.DoctorId,
                Name = doctor.User.FullName,
                Qualification = doctor.Qualification ?? string.Empty,
                ContactInfo = doctor.ContactInfo ?? string.Empty,
                Description = doctor.Description ?? string.Empty,
                Fees = doctor.Fees ?? 0,
                Review = doctor.Reviews.Any() ? doctor.Reviews.Average(r => r.Rating) : 0,
                WaitedTime = waitedTime
            };

            return response.CreateResponse(result);
        }


        #region Helper
        private TimeOnly CalculateWaitedTime(ICollection<Appointment> appointments)
        {
            var completedAppointments = appointments
                .Where(a => a.Status == AppointmentStatus.Completed && a.StartTime != null && a.EndTime != null)
                .ToList();

            if (!completedAppointments.Any())
            {
                return new TimeOnly(0, 30, 0); // Default 30 min wait time if no data available
            }

            var averageWaitTime = completedAppointments
                .Select(a => (a.EndTime.Value - a.StartTime.Value).TotalMinutes)
                .Average();

            return TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(averageWaitTime));
        }


        #endregion

    }
}
