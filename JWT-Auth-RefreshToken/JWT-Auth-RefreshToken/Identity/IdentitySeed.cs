using Microsoft.AspNetCore.Identity;

namespace JWT_Auth_RefreshToken.Identity;

public class IdentitySeed
{
    public static async Task SeedUserAsync(UserManager<AppUser> userManager)
    {
        var user = await userManager.FindByNameAsync("Alon");
        if (user is null)
        {
            user = new AppUser
            {
                UserName = "Alonso",
                Email = "email@email.com",
                
            };

            await userManager.CreateAsync(user, "Pass@123");
            
        }
    }
}
