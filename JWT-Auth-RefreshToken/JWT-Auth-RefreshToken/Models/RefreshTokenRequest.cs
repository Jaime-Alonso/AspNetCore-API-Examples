namespace JWT_Auth_RefreshToken.Models;

public record RefreshTokenRequest(string Email, string RefreshToken);
