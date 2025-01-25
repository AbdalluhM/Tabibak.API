using Microsoft.AspNetCore.Mvc;
using Tabibak.Api.BLL.Auth;
using Tabibak.Api.Dtos.AuthDtos;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthBLL _authBLL;

        public AuthController(IAuthBLL authBLL)
        {
            _authBLL = authBLL;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserInputDto inputDto)
        {
            var result = await _authBLL.RegisterAsync(inputDto);

            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginInputDto inputDto)
        {

            var result = await _authBLL.LoginAsync(inputDto);

            return Ok(result);
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshToken)
        {

            var result = await _authBLL.RefreshTokenAsync(refreshToken.RefreshToken);

            return Ok(result);
        }

    }
}
