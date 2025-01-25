using System.ComponentModel.DataAnnotations;
using Tabibak.Api.Enums;

namespace Tabibak.Api.Dtos.AuthDtos
{
    public class UserInputDto
    {
        public RoleEnum? Role { get; set; }
        public string UserName { get; set; }
        [Required, StringLength(100)]
        public string Email { get; set; }
        [Required, StringLength(20)]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }

}
