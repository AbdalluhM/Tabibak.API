using System.ComponentModel.DataAnnotations;

namespace Tabibak.Api.Dtos.AuthDtos
{
    public class ForgotPasswordDto
    {
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = null!;
    }

    public class VerifyResetCodeDto
    {
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = null!;

        [Required, StringLength(6, MinimumLength = 6)]
        public string Code { get; set; } = null!;
    }

    public class ResetPasswordDto
    {
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = null!;

        [Required, StringLength(6, MinimumLength = 6)]
        public string Code { get; set; } = null!;

        [Required, StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = null!;
    }
}
