using System.ComponentModel.DataAnnotations;

namespace Tabibak.Api.Dtos.AuthDtos
{
    public class ForgotPasswordDto
    {
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = null!;
    }
}
