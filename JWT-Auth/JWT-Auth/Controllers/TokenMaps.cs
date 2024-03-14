using JWT_Auth.Models;
using JWT_Auth.Services;

namespace JWT_Auth.Controllers;

public static class TokenMaps
{
    public static IEndpointRouteBuilder MapToken(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/token", (AuthenticationRequest request,TokenService service) =>
            service.GetToken(request.Email, request.Password));

        return endpoints;
    }
}
