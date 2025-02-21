using Tabibak.Api.BLL.BaseReponse;
using Tabibak.API.Dtos.Specialities;

namespace Tabibak.API.BLL.Doctors
{
    public interface IDoctorBLL
    {
        Task<IResponse<List<GetDoctoctorBySpecialtyDto>>> GetDoctorsBySpecialtyAsync(int specialtyId);
        Task<IResponse<DoctorDetailResultDto>> GetByIdAsync(int id);
    }
}
