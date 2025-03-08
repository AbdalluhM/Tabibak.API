using Microsoft.EntityFrameworkCore;
using Tabibak.Api.BLL.BaseReponse;
using Tabibak.Api.BLL.Constants;
using Tabibak.API.Core.Models;
using Tabibak.API.Dtos;
using Tabibak.API.Dtos.Reviews;
using Tabibak.API.Helpers.Enums;
using Tabibak.Context;

namespace Tabibak.API.BLL.Reviews
{
    public class ReviewBLL : IReviewBLL
    {
        private readonly ApplicationDbcontext _context;

        public ReviewBLL(ApplicationDbcontext context)
        {
            _context = context;
        }

        // ✅ Create a review (Patient can only review after an appointment)
        public async Task<IResponse<ReviewResponseDto>> CreateReviewAsync(CreateReviewDto dto)
        {
            var response = new Response<ReviewResponseDto>();

            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.DoctorId == dto.DoctorId);

            var patient = await _context.Patients
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.PatientId == dto.PatientId);

            if (doctor == null || patient == null)
                return response.CreateResponse(MessageCodes.NotFound, $"{nameof(doctor)} or {nameof(patient)}");

            // ✅ Check if patient has already reviewed this doctor
            bool alreadyReviewed = await _context.Reviews
                .AnyAsync(r => r.DoctorId == dto.DoctorId && r.PatientId == dto.PatientId);

            if (alreadyReviewed)
                return response.CreateResponse(MessageCodes.AlreadyExists, nameof(Review));

            // ✅ Check if the patient had an appointment with the doctor before reviewing
            bool hasAppointment = await _context.Appointments
                .AnyAsync(a => a.DoctorId == dto.DoctorId && a.PatientId == dto.PatientId && a.Status == AppointmentStatus.Completed);

            if (!hasAppointment)
                return response.CreateResponse(MessageCodes.ReviewPermission);

            var review = new Review
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                Rating = dto.Rating,
                Comments = dto.Comments,
                CreatedAt = DateTime.UtcNow,
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return response.CreateResponse(new ReviewResponseDto
            {
                ReviewId = review.ReviewId,
                DoctorId = review.DoctorId,
                DoctorName = doctor.Name,
                PatientId = review.PatientId,
                PatientName = patient.User.FullName,
                Rating = review.Rating,
                Comments = review.Comments,
                CreatedAt = review.CreatedAt,
            });
        }


        // ✅ Get all reviews
        public async Task<IResponse<List<ReviewResponseDto>>> GetAllReviewsAsync()
        {
            var response = new Response<List<ReviewResponseDto>>();
            return response.CreateResponse(await _context.Reviews
                .Include(r => r.Doctor)
                .ThenInclude(d => d.User)
                .Include(r => r.Patient)
                .ThenInclude(p => p.User)
                .Select(r => new ReviewResponseDto
                {
                    ReviewId = r.ReviewId,
                    DoctorId = r.DoctorId,
                    DoctorName = r.Doctor.Name,
                    PatientId = r.PatientId,
                    PatientName = r.Patient.User.FullName,
                    Rating = r.Rating,
                    Comments = r.Comments,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync());
        }

        // ✅ Get reviews by doctor
        public async Task<PagedResult<ReviewResponsePagedDto>> GetDoctorReviewsAsync(int doctorId, int pageNumber, int pageSize)
        {
            var query = _context.Reviews
                .Where(r => r.DoctorId == doctorId)
                .Include(r => r.Doctor)
                .ThenInclude(d => d.User)
                .Include(r => r.Patient)
                .ThenInclude(p => p.User)
                .OrderByDescending(r => r.CreatedAt);

            int totalCount = await query.CountAsync();

            // ✅ Calculate overall rating for the doctor
            double overallRating = await _context.Reviews
                // .Where(r => r.DoctorId == doctorId)
                .AverageAsync(r => (double?)r.Rating) ?? 0.0;

            var reviews = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new ReviewResponsePagedDto
                {
                    ReviewId = r.ReviewId,
                    DoctorId = r.DoctorId,
                    DoctorName = r.Doctor.Name,
                    PatientId = r.PatientId,
                    PatientName = r.Patient.User.FullName,
                    Rating = r.Rating,
                    Comments = r.Comments,
                    CreatedAt = r.CreatedAt,
                    OverAllRating = overallRating  // ✅ Add overall rating to each response
                })
                .ToListAsync();

            return new PagedResult<ReviewResponsePagedDto>
            {
                Items = reviews,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }


        public async Task<PagedResult<ReviewResponsePagedDto>> GetPatientReviewsAsync(int patientId, int pageNumber, int pageSize)
        {
            var query = _context.Reviews
                .Where(r => r.PatientId == patientId)
                 .Include(r => r.Doctor)
                .ThenInclude(d => d.User)
                .Include(r => r.Patient)
                .ThenInclude(p => p.User)
                .OrderByDescending(r => r.CreatedAt);

            int totalCount = await query.CountAsync();


            // ✅ Calculate overall rating for the doctor
            double overallRating = await _context.Reviews
                //.Where(r => r.PatientId == patientId)
                .AverageAsync(r => (double?)r.Rating) ?? 0.0;

            var reviews = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new ReviewResponsePagedDto
                {
                    ReviewId = r.ReviewId,
                    DoctorId = r.DoctorId,
                    DoctorName = r.Doctor.Name,
                    PatientId = r.PatientId,
                    PatientName = r.Patient.User.FullName,
                    Rating = r.Rating,
                    Comments = r.Comments,
                    CreatedAt = r.CreatedAt,
                    OverAllRating = overallRating,
                })
                .ToListAsync();

            return new PagedResult<ReviewResponsePagedDto>
            {
                Items = reviews,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<IResponse<bool>> UpdateReviewAsync(int reviewId, UpdateReviewDto dto)
        {
            var response = new Response<bool>();
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
                return response.CreateResponse(false);

            review.Rating = dto.Rating;
            review.Comments = dto.Comments;
            await _context.SaveChangesAsync();
            return response.CreateResponse(true);
        }

        // ✅ Delete a review
        public async Task<IResponse<bool>> DeleteReviewAsync(int reviewId)
        {
            var response = new Response<bool>();
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
                return response.CreateResponse(false);

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return response.CreateResponse(true); ;
        }
    }
}
