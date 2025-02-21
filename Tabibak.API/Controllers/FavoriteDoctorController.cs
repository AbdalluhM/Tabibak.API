using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabibak.API.BLL.Favourites;
using Tabibak.API.Dtos.Favourites;

namespace Tabibak.API.Controllers
{
    [Route("api/favorites")]
    [ApiController]
    [Authorize]
    public class FavoriteDoctorController : BaseController
    {
        private readonly IFavouriteDoctorBLL _favoriteDoctorService;

        public FavoriteDoctorController(IFavouriteDoctorBLL favoriteDoctorService)
        {
            _favoriteDoctorService = favoriteDoctorService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToFavorites([FromBody] FavoriteDoctorDto dto)
        {
            var result = await _favoriteDoctorService.AddToFavoritesAsync(UserId, dto.DoctorId);
            return Ok(result);
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromFavorites([FromBody] FavoriteDoctorDto dto)
        {
            var result = await _favoriteDoctorService.RemoveFromFavoritesAsync(UserId, dto.DoctorId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            var result = await _favoriteDoctorService.GetFavoritesByPatientIdAsync(UserId);
            return Ok(result);
        }

    }

}
