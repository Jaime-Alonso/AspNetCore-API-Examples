using Microsoft.AspNetCore.Identity;

namespace JWT_Auth_RefreshToken.Identity;

public class AppUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}
