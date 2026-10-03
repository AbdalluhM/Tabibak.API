using System.ComponentModel.DataAnnotations;

namespace Tabibak.Api.Dtos.AuthDtos
{
    public class ProfileDto
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class UpdateProfileDto
    {
        [Required, StringLength(100)]
        public string FullName { get; set; } = null!;

        [Required, StringLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = null!;
    }

    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = null!;

        [Required, StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = null!;
    }
}
