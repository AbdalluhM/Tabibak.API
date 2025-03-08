using Microsoft.AspNetCore.Mvc;
using Tabibak.API.BLL.Locations;

namespace Tabibak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationBLL _locationBLL;

        public LocationController(ILocationBLL locationService)
        {
            _locationBLL = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await _locationBLL.GetLocationsAsync();
            return Ok(locations);
        }
    }
}
