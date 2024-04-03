using JWT_Auth_RefreshToken.Models;
using JWT_Auth_RefreshToken.Services;

namespace JWT_Auth_RefreshToken.Controllers;

public static class TokenMaps
{
    public static IEndpointRouteBuilder MapToken(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/token", (AuthenticationRequest request, TokenService service) =>
            service.GetTokenAsync(request.Email, request.Password))
            .WithOpenApi()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        endpoints.MapPut("api/token", (RefreshTokenRequest request, TokenService service) =>
            service.RefreshTokenAsync(request.Email, request.RefreshToken))
            .WithOpenApi()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        return endpoints;
    }
}
