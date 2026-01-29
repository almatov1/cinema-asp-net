namespace Cinema.Domain.DTOs;

public sealed record AuthResponse(
    string AccessToken,
    string RefreshToken
);
