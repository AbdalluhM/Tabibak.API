using Tabibak.Api.BLL.BaseReponse;
using Tabibak.API.Dtos.Reviews;

namespace Tabibak.API.BLL.Reviews
{
    public interface IReviewBLL
    {
        Task<IResponse<ReviewResponseDto>> CreateReviewAsync(CreateReviewDto dto);
        Task<IResponse<List<ReviewResponseDto>>> GetAllReviewsAsync();
        Task<IResponse<List<ReviewResponseDto>>> GetReviewsByDoctorAsync(int doctorId);
        Task<IResponse<List<ReviewResponseDto>>> GetReviewsByPatientAsync(int patientId);
        Task<IResponse<bool>> UpdateReviewAsync(int reviewId, UpdateReviewDto dto);
        Task<IResponse<bool>> DeleteReviewAsync(int reviewId);
    }
}
