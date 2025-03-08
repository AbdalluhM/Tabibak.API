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
        public async Task<IResponse<List<GetDoctorBySpecialtyDto>>> GetDoctorsBySpecialtyAsync(int patientId, int specialtyId,
                                                                                                    int? locationId = null,
                                                                                                    bool acceptPromoCode = false,
                                                                                                    decimal? minFees = null,
                                                                                                    decimal? maxFees = null,
                                                                                                    DateTime? dateFilter = null,
                                                                                                    string gender = null)
        {
            var response = new Response<List<GetDoctorBySpecialtyDto>>();
            // Start building query
            var query = _dbcontext.Doctors
                .Include(d => d.User)
                .Include(d => d.DoctorSpecialties)
                .ThenInclude(ds => ds.Specialty)
                .Include(d => d.Appointments)
                .Where(d => d.DoctorSpecialties.Any(ds => ds.SpecialtyId == specialtyId))
                .AsQueryable();

            // ✅ Filter by Location
            if (locationId > 0)
            {
                query = query.Where(d => d.LocationId == locationId);
            }

            // ✅ Filter by Examination Fees Range
            if (minFees.HasValue)
            {
                query = query.Where(d => d.Fees >= minFees);
            }
            if (maxFees.HasValue)
            {
                query = query.Where(d => d.Fees <= maxFees);
            }

            // ✅ Filter by Available Dates (if appointments exist)
            if (dateFilter.HasValue)
            {
                query = query.Where(d => d.Appointments.Any(a => a.AppointmentDate.Date == dateFilter.Value.Date));
            }

            // ✅ Filter by Gender
            if (!string.IsNullOrEmpty(gender) && gender.ToLower() != "all")
            {
                if (Enum.TryParse(typeof(GenderEnum), gender, true, out var genderEnum))
                {
                    query = query.Where(d => d.Gender == (GenderEnum)genderEnum);
                }
            }


            // ✅ Filter by Promo Code Support (Assuming doctors have this option)
            if (acceptPromoCode)
            {
                query = query.Where(d => d.AcceptPromoCode);
            }

            var doctors = await query.ToListAsync();
            var mappedDoctors = _mapper.Map<List<GetDoctorBySpecialtyDto>>(doctors);

            // ✅ Get the list of favorite doctors for this user
            var favoriteDoctorIds = await _dbcontext.FavoriteDoctors
                .Where(f => f.PatientId == patientId)
                .Select(f => f.DoctorId)
                .ToListAsync();

            // ✅ Update IsFavorite field based on user favorites
            foreach (var doctor in mappedDoctors)
            {
                doctor.IsFavorite = favoriteDoctorIds.Contains(doctor.DoctorId);
            }

            return response.CreateResponse(mappedDoctors);
        }
        public async Task<IResponse<DoctorDetailResultDto>> GetDoctorDetailsAsync(int doctorId, int patientId)
        {
            var response = new Response<DoctorDetailResultDto>();

            var doctor = await _dbcontext.Doctors
                .Where(d => d.DoctorId == doctorId)
                .Include(d => d.User)
                .Include(d => d.Reviews)
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync();

            var review = _dbcontext.Patients.Include(p => p.Reviews)
                                                    .FirstOrDefault(p => p.PatientId == patientId)?
                                                    .Reviews.Where(r => r.DoctorId == doctorId).FirstOrDefault();

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
                HasReview = review is not null,
                PatienReview = review?.Rating ?? 0,
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
