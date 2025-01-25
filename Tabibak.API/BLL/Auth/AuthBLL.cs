using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Tabibak.Api.BLL.BaseReponse;
using Tabibak.Api.BLL.Constants;
using Tabibak.Api.Dtos.AuthDtos;
using Tabibak.Api.Enums;
using Tabibak.Api.Helpers.Settings;
using Tabibak.Models;

namespace Tabibak.Api.BLL.Auth
{
    public class AuthBLL : BaseBLL, IAuthBLL
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly JWT _jwt;
        public AuthBLL(UserManager<ApplicationUser> userManager, IMapper mapper, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwt = jwt.Value;
        }

        public async Task<IResponse<LoginResultDto>> LoginAsync(LoginInputDto inputDto)
        {
            var response = new Response<LoginResultDto>();
            try
            {
                ApplicationUser user = new();
                if (inputDto.Email.Contains("@"))
                {
                    user = await _userManager.FindByEmailAsync(inputDto.Email);
                }
                else
                {
                    user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == inputDto.Email);
                }


                if (user == null || !await _userManager.CheckPasswordAsync(user, inputDto.Password))
                    return response.CreateResponse(MessageCodes.InvalidLoginCredentials);

                var token = await CreateJwtToken(user);
                string refreshToken = string.Empty;
                DateTime refreshDateExpiration = default;

                if (user.RefreshTokens.Any(t => t.IsActive))
                {
                    var refreshTokenDb = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                    refreshToken = refreshTokenDb.Token;
                    refreshDateExpiration = refreshTokenDb.ExpiresOn;
                }
                else
                {
                    var newRefreshToken = CreateRefreshToken();
                    user.RefreshTokens.Add(newRefreshToken);
                    await _userManager.UpdateAsync(user);
                    refreshToken = newRefreshToken.Token;
                    refreshDateExpiration = newRefreshToken.ExpiresOn;

                }
                var roles = await _userManager.GetRolesAsync(user);
                return response.CreateResponse(new LoginResultDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    RefreshDateExpiration = refreshDateExpiration,
                    Role = roles.FirstOrDefault() ?? string.Empty,
                });
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<IResponse<LoginResultDto>> RefreshTokenAsync(string token)
        {
            var response = new Response<LoginResultDto>();
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
                return response.CreateResponse(MessageCodes.InvalidToken);

            var refreshToken = user.RefreshTokens?.Single(t => t.Token == token);

            if (refreshToken != null && !refreshToken.IsActive)
                return response.CreateResponse(MessageCodes.InvalidToken);

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = CreateRefreshToken();
            var jwtToken = await CreateJwtToken(user);

            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            return response.CreateResponse(new LoginResultDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = newRefreshToken.Token,
                RefreshDateExpiration = newRefreshToken.ExpiresOn

            });
        }
        public async Task<IResponse<bool>> RegisterAsync(UserInputDto inputDto)
        {
            var response = new Response<bool>();

            try
            {
                if (await _userManager.FindByEmailAsync(inputDto.Email) is not null)
                    return response.CreateResponse(MessageCodes.AlreadyExists, inputDto.Email);

                if (await _userManager.FindByNameAsync(inputDto.UserName) is not null)
                    return response.CreateResponse(MessageCodes.AlreadyExists, inputDto.UserName);

                var user = _mapper.Map<ApplicationUser>(inputDto);

                var result = await _userManager.CreateAsync(user, inputDto.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        response.AppendError(new TErrorField
                        {
                            Code = error.Code,
                            Message = error.Description,
                        });
                    }
                    return response.CreateResponse();
                }

                if (inputDto.Role != null)
                {
                    await AssignRoleToUser(inputDto.Role, user);
                }



                return response.CreateResponse(true);
            }
            catch (Exception e)
            {

                throw;
            }


        }
        public async Task<IResponse<bool>> RevokeTokenAsync(string token)
        {
            var output = new GetAuthOutputDto();
            var response = new Response<bool>();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
                return response.CreateResponse(false);

            var refreshToken = user.RefreshTokens?.Single(t => t.Token == token);
            if (refreshToken != null && !refreshToken.IsActive)
                return response.CreateResponse(false);

            refreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return response.CreateResponse(true);
        }

        public async Task AssignRoleToUser(RoleEnum? role, ApplicationUser user)
        {
            switch (role)
            {
                case RoleEnum.BusinessAdmin:
                    await _userManager.AddToRoleAsync(user, nameof(RoleEnum.BusinessAdmin));
                    break;
                //case RoleEnum.Admin:
                //    await _userManager.AddToRoleAsync(user, nameof(RoleEnum.Admin));
                //    break;
                case RoleEnum.Customer:
                    await _userManager.AddToRoleAsync(user, nameof(RoleEnum.Customer));
                    break;
                default:
                    await _userManager.AddToRoleAsync(user, nameof(RoleEnum.Customer));
                    break;
            }
        }


        #region CreatTokenJwt And RefreshToken
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audiance,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.ExpireOn),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }


        private RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var genrator = new RNGCryptoServiceProvider();
            genrator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(1),
                CreatedOn = DateTime.UtcNow
            };
        }
        #endregion
    }
}
