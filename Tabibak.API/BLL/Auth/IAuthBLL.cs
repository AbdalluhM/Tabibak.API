using Tabibak.Api.BLL.BaseReponse;
using Tabibak.Api.Dtos.AuthDtos;

namespace Tabibak.Api.BLL.Auth
{
    public interface IAuthBLL
    {
        Task<IResponse<bool>> RegisterAsync(UserInputDto inputDto);
        Task<IResponse<LoginResultDto>> LoginAsync(LoginInputDto inputDto);
        Task<IResponse<LoginResultDto>> RefreshTokenAsync(string token);
        Task<IResponse<bool>> RevokeTokenAsync(string token);
        Task<IResponse<ProfileDto>> GetProfileAsync(string userId);
        Task<IResponse<ProfileDto>> UpdateProfileAsync(string userId, UpdateProfileDto inputDto);
        Task<IResponse<bool>> ChangePasswordAsync(string userId, ChangePasswordDto inputDto);
        Task<IResponse<bool>> ForgotPasswordAsync(ForgotPasswordDto inputDto);
    }
}
