using System.ComponentModel.DataAnnotations;
using Tabibak.Api.Enums;

namespace Tabibak.Api.Dtos.AuthDtos
{
    public class UserInputDto
    {
        public RoleEnum? Role { get; set; }
        public string FullName { get; set; } = null!;
        [Required, StringLength(100)]
        public string Email { get; set; }
        [Required, StringLength(20)]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DoctorInputDto? Doctor { get; set; } = null;
        public PatientInputDto? Patient { get; set; } = null;
    }
    public class DoctorInputDto
    {
        public string Qualification { get; set; }
        public string ContactInfo { get; set; }
        public List<SpecialtyInputDto> Specialties { get; set; } = new();

    }
    public class PatientInputDto
    {

    }
    public class SpecialtyInputDto
    {
        public int SpecialtyId { get; set; }
    }

}
