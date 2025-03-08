using Tabibak.Api.BLL.BaseReponse;
using Tabibak.API.Dtos.Specialities;

namespace Tabibak.API.BLL.Doctors
{
    public interface IDoctorBLL
    {
        Task<IResponse<List<GetDoctorBySpecialtyDto>>> GetDoctorsBySpecialtyAsync(int userId, int specialtyId,
                                                                                                     int? location = null,
                                                                                                     bool acceptPromoCode = false,
                                                                                                     decimal? minFees = null,
                                                                                                     decimal? maxFees = null,
                                                                                                     DateTime? dateFilter = null,
                                                                                                     string gender = null);
        Task<IResponse<DoctorDetailResultDto>> GetDoctorDetailsAsync(int doctorId, int userId);
    }
}
