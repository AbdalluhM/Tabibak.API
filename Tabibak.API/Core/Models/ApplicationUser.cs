using Microsoft.AspNetCore.Identity;
using Tabibak.API.Core.Models;

namespace Tabibak.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Role { get; set; }  // "Patient" or "Doctor"

        // ✅ Relationship with Doctor and Patient (One-to-One)
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }


    }
}
