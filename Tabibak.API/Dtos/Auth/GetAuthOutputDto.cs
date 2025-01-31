using System.Text.Json.Serialization;

namespace Tabibak.Api.Dtos.AuthDtos
{
    public class GetAuthOutputDto
    {
        public string Message { get; set; }
        public bool IsAuthentication { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
        public string Token { get; set; }
        //public DateTime ExpireOn { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public DateTime RefreshDateExpiration { get; set; }
    }

    public class LoginResultDto
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime RefreshDateExpiration { get; set; }
    }


}
