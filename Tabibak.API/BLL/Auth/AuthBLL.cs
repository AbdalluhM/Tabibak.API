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
using Tabibak.API.Core.Models;
using Tabibak.Context;
using Tabibak.Models;

namespace Tabibak.Api.BLL.Auth
{
    public class AuthBLL : BaseBLL, IAuthBLL
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbcontext _context;
        private readonly IMapper _mapper;
        private readonly JWT _jwt;
        public AuthBLL(UserManager<ApplicationUser> userManager, IMapper mapper, IOptions<JWT> jwt, ApplicationDbcontext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwt = jwt.Value;
            _context = context;
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

                int patienOrDoctorId = 0;
                if (user.Role == nameof(RoleEnum.Doctor))
                {
                    var doctor = await _context.Doctors.FirstOrDefaultAsync(u => u.UserId == user.Id);
                    patienOrDoctorId = doctor?.DoctorId ?? 0;
                }
                else
                {
                    var patient = await _context.Patients.FirstOrDefaultAsync(u => u.UserId == user.Id);
                    patienOrDoctorId = patient?.PatientId ?? 0;
                }

                var token = await CreateJwtToken(user, patienOrDoctorId);
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
            int patienOrDoctorId = 0;
            if (user.Role == nameof(RoleEnum.Doctor))
            {
                var doctor = await _context.Doctors.FindAsync(user.Id);
                patienOrDoctorId = doctor?.DoctorId ?? 0;
            }
            else
            {
                var patient = await _context.Patients.FindAsync(user.Id);
                patienOrDoctorId = patient?.PatientId ?? 0;
            }
            var jwtToken = await CreateJwtToken(user, patienOrDoctorId);

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

                if (await _userManager.FindByNameAsync(inputDto.FullName) is not null)
                    return response.CreateResponse(MessageCodes.AlreadyExists, inputDto.FullName);

                //   var user = _mapper.Map<ApplicationUser>(inputDto);

                var user = new ApplicationUser
                {
                    FullName = inputDto.FullName,
                    UserName = inputDto.FullName.Split(" ")[0],
                    Email = inputDto.Email,
                    PhoneNumber = inputDto.PhoneNumber,
                    Role = inputDto.Role switch
                    {
                        RoleEnum.Patient => RoleEnum.Patient.ToString(),
                        RoleEnum.Doctor => RoleEnum.Doctor.ToString(),
                        _ => RoleEnum.Patient.ToString()
                    }
                };

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

                // ✅ Create Doctor or Patient record
                if (inputDto.Role == RoleEnum.Doctor)
                {
                    var mappedDoctor = _mapper.Map<Doctor>(inputDto.Doctor);
                    mappedDoctor.UserId = user.Id;

                    _context.Add(mappedDoctor);
                }

                else if (inputDto.Role == RoleEnum.Patient)
                {
                    _context.Add(new Patient
                    {
                        UserId = user.Id,
                    });
                }

                await _context.SaveChangesAsync();

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
                case RoleEnum.Patient:
                    await _userManager.AddToRoleAsync(user, nameof(RoleEnum.Patient));
                    break;
                //case RoleEnum.Admin:
                //    await _userManager.AddToRoleAsync(user, nameof(RoleEnum.Admin));
                //    break;
                case RoleEnum.Doctor:
                    await _userManager.AddToRoleAsync(user, nameof(RoleEnum.Doctor));
                    break;
                default:
                    await _userManager.AddToRoleAsync(user, nameof(RoleEnum.Patient));
                    break;
            }
        }


        #region CreatTokenJwt And RefreshToken
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user, int pateientOrDoctorId)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            string userId = pateientOrDoctorId.ToString();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(nameof(userId), userId),
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
