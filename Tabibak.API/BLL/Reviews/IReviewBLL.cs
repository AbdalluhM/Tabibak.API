using Tabibak.Api.BLL.BaseReponse;
using Tabibak.API.Dtos;
using Tabibak.API.Dtos.Reviews;

namespace Tabibak.API.BLL.Reviews
{
    public interface IReviewBLL
    {
        Task<IResponse<ReviewResponseDto>> CreateReviewAsync(CreateReviewDto dto);
        Task<IResponse<List<ReviewResponseDto>>> GetAllReviewsAsync();
        Task<PagedResult<ReviewResponsePagedDto>> GetDoctorReviewsAsync(int doctorId, int pageNumber, int pageSize);
        //Task<IResponse<List<ReviewResponsePagedDto>>> GetReviewsByDoctorAsync(int doctorId);
        //Task<IResponse<List<ReviewResponseDto>>> GetReviewsByPatientAsync(int patientId);
        Task<PagedResult<ReviewResponsePagedDto>> GetPatientReviewsAsync(int patientId, int pageNumber, int pageSize);
        Task<IResponse<bool>> UpdateReviewAsync(int reviewId, UpdateReviewDto dto);
        Task<IResponse<bool>> DeleteReviewAsync(int reviewId);
    }
}
