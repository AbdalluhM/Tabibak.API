using System.ComponentModel.DataAnnotations;

namespace Tabibak.Api.Dtos.AuthDtos
{
    public class LoginInputDto
    {
        [Required, StringLength(100)]
        public string Email { get; set; }
        [Required, StringLength(20)]
        public string Password { get; set; }
    }

}
