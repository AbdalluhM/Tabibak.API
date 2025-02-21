using Tabibak.Api.BLL.BaseReponse;
using Tabibak.API.Dtos.Favourites;

namespace Tabibak.API.BLL.Favourites
{
    public interface IFavouriteDoctorBLL
    {
        Task<IResponse<bool>> AddToFavoritesAsync(int patientId, int doctorId);
        Task<IResponse<bool>> RemoveFromFavoritesAsync(int patientId, int doctorId);
        Task<IResponse<List<DoctorResponseDto>>> GetFavoritesByPatientIdAsync(int patientId);
    }
}
