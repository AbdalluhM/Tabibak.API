using Tabibak.Api.BLL.BaseReponse;

namespace Tabibak.API.BLL.Favourites
{
    public interface IFavouriteDoctorBLL
    {
        Task<IResponse<bool>> AddToFavoritesAsync(int patientId, int doctorId);
        Task<IResponse<bool>> RemoveFromFavoritesAsync(int patientId, int doctorId);
    }
}
