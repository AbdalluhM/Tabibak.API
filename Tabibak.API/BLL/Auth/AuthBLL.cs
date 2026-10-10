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
using Tabibak.Api.Helpers.Email;
using Tabibak.Api.Helpers.Settings;
using Tabibak.API.Core.Models;
using Tabibak.Context;
using Tabibak.Models;

namespace Tabibak.Api.BLL.Auth
{
    public class AuthBLL : BaseBLL, IAuthBLL
    {
        private const string PasswordResetProvider = "Tabibak";
        private const string PasswordResetTokenName = "PasswordResetCode";
        private static readonly TimeSpan PasswordResetCodeLifetime = TimeSpan.FromMinutes(15);

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbcontext _context;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AuthBLL> _logger;
        private readonly JWT _jwt;
        public AuthBLL(UserManager<ApplicationUser> userManager, IMapper mapper, IOptions<JWT> jwt, ApplicationDbcontext context, IEmailSender emailSender, ILogger<AuthBLL> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwt = jwt.Value;
            _context = context;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<IResponse<LoginResultDto>> LoginAsync(LoginInputDto inputDto)
        {
            var response = new Response<LoginResultDto>();
            try
            {
                ApplicationUser? user;
                if (inputDto.Email.Contains("@"))
                {
                    var email = _userManager.NormalizeEmail(inputDto.Email);
                    user = await _userManager.Users
                        .Include(u => u.RefreshTokens)
                        .FirstOrDefaultAsync(u => u.NormalizedEmail == email);
                }
                else
                {
                    user = await _userManager.Users
                        .Include(u => u.RefreshTokens)
                        .FirstOrDefaultAsync(u => u.PhoneNumber == inputDto.Email);
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

                user.RefreshTokens ??= new List<RefreshToken>();
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
            if (string.IsNullOrWhiteSpace(token))
                return response.CreateResponse(MessageCodes.InvalidToken);

            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
                return response.CreateResponse(MessageCodes.InvalidToken);

            user.RefreshTokens ??= new List<RefreshToken>();
            var refreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == token);
            if (refreshToken == null || !refreshToken.IsActive)
                return response.CreateResponse(MessageCodes.InvalidToken);

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = CreateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);

            int patienOrDoctorId = 0;
            if (user.Role == nameof(RoleEnum.Doctor))
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == user.Id);
                patienOrDoctorId = doctor?.DoctorId ?? 0;
            }
            else
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == user.Id);
                patienOrDoctorId = patient?.PatientId ?? 0;
            }

            var jwtToken = await CreateJwtToken(user, patienOrDoctorId);
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.UpdateAsync(user);

            return response.CreateResponse(new LoginResultDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = newRefreshToken.Token,
                RefreshDateExpiration = newRefreshToken.ExpiresOn,
                Role = roles.FirstOrDefault() ?? user.Role ?? string.Empty,
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
                    _context.Add(new Doctor
                    {
                        UserId = user.Id,
                    });
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

        public async Task<IResponse<ProfileDto>> GetProfileAsync(string userId)
        {
            var response = new Response<ProfileDto>();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return response.CreateResponse(MessageCodes.NotFound, nameof(ApplicationUser));

            return response.CreateResponse(ToProfileDto(user));
        }

        public async Task<IResponse<ProfileDto>> UpdateProfileAsync(string userId, UpdateProfileDto inputDto)
        {
            var response = new Response<ProfileDto>();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return response.CreateResponse(MessageCodes.NotFound, nameof(ApplicationUser));

            var email = inputDto.Email.Trim();
            var phoneNumber = inputDto.PhoneNumber.Trim();

            var emailOwner = await _userManager.FindByEmailAsync(email);
            if (emailOwner != null && emailOwner.Id != user.Id)
                return response.CreateResponse(MessageCodes.EmailAlreadyExists);

            var phoneTaken = await _userManager.Users.AnyAsync(u => u.PhoneNumber == phoneNumber && u.Id != user.Id);
            if (phoneTaken)
                return response.CreateResponse(MessageCodes.AlreadyExists, nameof(ApplicationUser.PhoneNumber));

            user.FullName = inputDto.FullName.Trim();
            user.PhoneNumber = phoneNumber;
            user.Email = email;
            user.NormalizedEmail = _userManager.NormalizeEmail(email);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return IdentityErrorResponse(response, result);

            return response.CreateResponse(ToProfileDto(user));
        }

        public async Task<IResponse<bool>> ChangePasswordAsync(string userId, ChangePasswordDto inputDto)
        {
            var response = new Response<bool>();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return response.CreateResponse(MessageCodes.NotFound, nameof(ApplicationUser));

            if (inputDto.CurrentPassword == inputDto.NewPassword)
                return response.CreateResponse(MessageCodes.NewPasswordAlreadyDefined);

            var result = await _userManager.ChangePasswordAsync(user, inputDto.CurrentPassword, inputDto.NewPassword);
            if (!result.Succeeded)
            {
                if (result.Errors.Any(e => e.Code == "PasswordMismatch"))
                    return response.CreateResponse(MessageCodes.InvalidPassword);

                return IdentityErrorResponse(response, result);
            }

            return response.CreateResponse(true);
        }

        public async Task<IResponse<bool>> ForgotPasswordAsync(ForgotPasswordDto inputDto)
        {
            var response = new Response<bool>();
            var user = await _userManager.FindByEmailAsync(inputDto.Email.Trim());
            if (user == null)
                return response.CreateResponse(MessageCodes.NotFound, nameof(ApplicationUser.Email));

            var code = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
            var expiresOn = DateTime.UtcNow.Add(PasswordResetCodeLifetime);
            var payload = $"{HashResetCode(code)}|{expiresOn:O}";
            await _userManager.SetAuthenticationTokenAsync(user, PasswordResetProvider, PasswordResetTokenName, payload);

            try
            {
                await _emailSender.SendAsync(
                    user.Email!,
                    "Tabibak password reset code",
                    $"Your verification code is {code}. It expires in 15 minutes.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset code to {Email}", user.Email);
                return response.CreateResponse(MessageCodes.EmailSendFailed);
            }

            return response.CreateResponse(true);
        }

        public async Task<IResponse<bool>> VerifyResetCodeAsync(VerifyResetCodeDto inputDto)
        {
            var response = new Response<bool>();
            var validation = await ValidateResetCodeAsync(inputDto.Email, inputDto.Code);
            if (!validation.IsSuccess)
                return response.CreateResponse(validation.ErrorCode!.Value, validation.ErrorMessage ?? string.Empty);

            return response.CreateResponse(true);
        }

        public async Task<IResponse<bool>> ResetPasswordAsync(ResetPasswordDto inputDto)
        {
            var response = new Response<bool>();
            var validation = await ValidateResetCodeAsync(inputDto.Email, inputDto.Code);
            if (!validation.IsSuccess)
                return response.CreateResponse(validation.ErrorCode!.Value, validation.ErrorMessage ?? string.Empty);

            var user = validation.User!;
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, inputDto.NewPassword);
            if (!result.Succeeded)
                return IdentityErrorResponse(response, result);

            await _userManager.RemoveAuthenticationTokenAsync(user, PasswordResetProvider, PasswordResetTokenName);

            if (user.RefreshTokens != null)
            {
                foreach (var refreshToken in user.RefreshTokens.Where(t => t.IsActive))
                    refreshToken.RevokedOn = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }

            return response.CreateResponse(true);
        }

        private async Task<(bool IsSuccess, ApplicationUser? User, MessageCodes? ErrorCode, string? ErrorMessage)> ValidateResetCodeAsync(string email, string code)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.NormalizedEmail == _userManager.NormalizeEmail(email.Trim()));

            if (user == null)
                return (false, null, MessageCodes.NotFound, nameof(ApplicationUser.Email));

            var payload = await _userManager.GetAuthenticationTokenAsync(user, PasswordResetProvider, PasswordResetTokenName);
            if (string.IsNullOrWhiteSpace(payload))
                return (false, null, MessageCodes.InvalidVerificationLink, string.Empty);

            var parts = payload.Split('|', 2);
            if (parts.Length != 2 || !DateTime.TryParse(parts[1], null, System.Globalization.DateTimeStyles.RoundtripKind, out var expiresOn))
                return (false, null, MessageCodes.InvalidVerificationLink, string.Empty);

            if (DateTime.UtcNow > expiresOn)
            {
                await _userManager.RemoveAuthenticationTokenAsync(user, PasswordResetProvider, PasswordResetTokenName);
                return (false, null, MessageCodes.PhoneCodeExpired, string.Empty);
            }

            if (!string.Equals(parts[0], HashResetCode(code.Trim()), StringComparison.Ordinal))
                return (false, null, MessageCodes.InvalidVerificationLink, string.Empty);

            return (true, user, null, null);
        }

        private static string HashResetCode(string code)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(code));
            return Convert.ToBase64String(bytes);
        }

        private static ProfileDto ToProfileDto(ApplicationUser user)
        {
            return new ProfileDto
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Email = user.Email ?? string.Empty
            };
        }

        private static IResponse<T> IdentityErrorResponse<T>(Response<T> response, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                response.AppendError(new TErrorField
                {
                    Code = error.Code,
                    Message = error.Description
                });
            }

            return response.CreateResponse();
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
