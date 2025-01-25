﻿using Microsoft.AspNetCore.Identity;

namespace Tabibak.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<RefreshToken>? RefreshTokens { get; set; }


    }
}
