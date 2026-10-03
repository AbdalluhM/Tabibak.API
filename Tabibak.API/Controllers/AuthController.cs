using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto inputDto)
        {
            var result = await _authBLL.ForgotPasswordAsync(inputDto);
            return Ok(result);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshToken)
        {

            var result = await _authBLL.RefreshTokenAsync(refreshToken.RefreshToken);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = CurrentUserId();
            if (userId == null)
                return Unauthorized();

            var result = await _authBLL.GetProfileAsync(userId);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto inputDto)
        {
            var userId = CurrentUserId();
            if (userId == null)
                return Unauthorized();

            var result = await _authBLL.UpdateProfileAsync(userId, inputDto);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto inputDto)
        {
            var userId = CurrentUserId();
            if (userId == null)
                return Unauthorized();

            var result = await _authBLL.ChangePasswordAsync(userId, inputDto);
            return Ok(result);
        }

        private string? CurrentUserId()
        {
            return User.FindFirstValue("uid");
        }

    }
}
