using JWT_Auth_RefreshToken.Configuration;
using JWT_Auth_RefreshToken.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_Auth_RefreshToken.Services;

public class TokenService
{
    private readonly JwtConfigurationModel _jwtConfig;
    private readonly UserManager<AppUser> _userManager;    

    public TokenService(IOptions<JwtConfigurationModel> jwtConfig, UserManager<AppUser> userManager)
    {
        _jwtConfig = jwtConfig.Value;
        _userManager = userManager;
    }

    public async Task<IResult> GetTokenAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, password))
        {
            return Results.Unauthorized();
        }        

        return await GenerateTokensAsync(user);
    }

    public async Task<IResult> RefreshTokenAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null || user.RefreshToken != token || user.RefreshTokenExpiration < DateTime.Now)
        {            
            return Results.Unauthorized();
        }

        /*I prefer regenerate refresh token too*/
        return await GenerateTokensAsync(user);
    }

    private async Task<IResult> GenerateTokensAsync(AppUser user)
    {
        var jwt = GenerateJWTAccessToken();
        await GenerateRefreshTokenAsync(user);

        return Results.Ok(new
        {
            Email = user.Email!,
            AccessToken = jwt.token,
            AccessTokenExpiration = jwt.expiration,
            RefreshToken = user.RefreshToken!,
            RefreshTokenExpiration = user.RefreshTokenExpiration!
        });
    }

    private (string token, DateTime expiration) GenerateJWTAccessToken()
    {
        var claims = GenerateClaims();
        var expiration = DateTime.Now.AddMinutes(_jwtConfig.AccessTokenExpirationInMinutes);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return (jwt, expiration);
    }

    private async Task GenerateRefreshTokenAsync(AppUser user)
    {
        user.RefreshToken = Guid.NewGuid().ToString();
        user.RefreshTokenExpiration = DateTime.Now.AddDays(_jwtConfig.RefreshTokenExpirationInDays).ToUniversalTime();
        await _userManager.UpdateAsync(user);
    }

    

    private IEnumerable<Claim> GenerateClaims()
    {
        var claims = new List<Claim>();

        //Example claims:
        //claims.Add(new Claim(ClaimTypes.Email, user.Email));
        //claims.Add(new Claim(ClaimTypes.Name, user.Name));

        //Roles claims Example:
        //var roles = logic for obtain rolenames
        //foreach (var role in roles)
        //{
        //    claims.Add(new Claim(ClaimTypes.Role, role));
        //}

        return claims;
    }    
}
