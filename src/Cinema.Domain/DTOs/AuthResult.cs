namespace Cinema.Domain.DTOs;

public sealed record AuthResult(
    string AccessToken,
    string RefreshToken
);
