using Tabibak.Api.BLL.BaseReponse;
using Tabibak.API.Dtos.Locations;

namespace Tabibak.API.BLL.Locations
{
    public interface ILocationBLL
    {
        Task<IResponse<List<LocationDto>>> GetLocationsAsync();
    }
}
