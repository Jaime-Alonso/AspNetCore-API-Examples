using JWT_Auth.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_Auth.Services;

public class TokenService
{
    private readonly JwtConfigurationModel _jwtConfig;

    public TokenService(IOptions<JwtConfigurationModel> jwtConfig)
    {
        _jwtConfig = jwtConfig.Value;
    }

    public IResult GetToken(string email, string password)
    {
        //your logic for login process
        //If email and password are correct then proceed to generate token otherwise return Results.Unauthorize();

        //Important Note: 
        //never return "username does not exist or password is incorrect", always return "username or password is incorrect" for security reasons


        var claims = GenerateClaims();

        var expiration = DateTime.Now.AddMinutes(_jwtConfig.ExpirationInMinutes);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return Results.Ok(new 
        { 
            AccessToken = jwt,
            Expiration = expiration
        });
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
