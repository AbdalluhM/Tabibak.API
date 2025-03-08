using Microsoft.EntityFrameworkCore;
using Tabibak.Api.BLL;
using Tabibak.Api.BLL.BaseReponse;
using Tabibak.API.Dtos.Locations;
using Tabibak.Context;

namespace Tabibak.API.BLL.Locations
{
    public class LocationBLL : BaseBLL, ILocationBLL
    {
        private readonly ApplicationDbcontext _dbContext;

        public LocationBLL(ApplicationDbcontext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IResponse<List<LocationDto>>> GetLocationsAsync()
        {
            var response = new Response<List<LocationDto>>();
            var locations = await _dbContext.Locations
                .Select(l => new LocationDto { LocationId = l.LocationId, Name = l.Name })
                .ToListAsync();

            return response.CreateResponse(locations);
        }
    }
}
