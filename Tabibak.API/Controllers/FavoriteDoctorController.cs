using Microsoft.AspNetCore.Mvc;
using Tabibak.API.BLL.Favourites;
using Tabibak.API.Dtos.Favourites;

namespace Tabibak.API.Controllers
{
    [Route("api/favorites")]
    [ApiController]
    public class FavoriteDoctorController : ControllerBase
    {
        private readonly IFavouriteDoctorBLL _favoriteDoctorService;

        public FavoriteDoctorController(IFavouriteDoctorBLL favoriteDoctorService)
        {
            _favoriteDoctorService = favoriteDoctorService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToFavorites([FromBody] FavoriteDoctorDto dto)
        {
            var result = await _favoriteDoctorService.AddToFavoritesAsync(dto.PatientId, dto.DoctorId);
            return Ok(result);
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromFavorites([FromBody] FavoriteDoctorDto dto)
        {
            var result = await _favoriteDoctorService.RemoveFromFavoritesAsync(dto.PatientId, dto.DoctorId);
            return Ok(result);
        }
    }

}
