using Cinema.Domain.DTOs;
using Cinema.Domain.Entities;
using Cinema.Domain.Interfaces;

namespace Cinema.Application.Services;

public sealed class AuthService(IUserRepository userRepo, IRefreshTokenRepository refreshRepo, IJwtTokenService jwtService)
{
    private readonly IUserRepository _userRepo = userRepo;
    private readonly IRefreshTokenRepository _refreshRepo = refreshRepo;
    private readonly IJwtTokenService _jwtService = jwtService;

    public async Task<AuthResult> LoginAsync(string login, string password)
    {
        var user = await _userRepo.GetByLoginAsync(login)
             ?? throw new UnauthorizedAccessException("Invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        var accessToken = _jwtService.GenerateToken(user);
        var refreshTokenValue = _jwtService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(14)
        };

        await _refreshRepo.CreateAsync(refreshToken);
        return new AuthResult(accessToken, refreshTokenValue);
    }

    public async Task<AuthResult> RefreshAsync(string token)
    {
        var existingToken = await _refreshRepo.GetAsync(token)
            ?? throw new UnauthorizedAccessException("Invalid refresh token");

        if (existingToken.ExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Token expired");

        var user = await _userRepo.GetByIdAsync(existingToken.UserId)
            ?? throw new UnauthorizedAccessException("User not found");

        await _refreshRepo.RevokeAsync(existingToken.Id);

        var newAccess = _jwtService.GenerateToken(user);
        var newRefreshValue = _jwtService.GenerateRefreshToken();

        await _refreshRepo.CreateAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = newRefreshValue,
            ExpiresAt = DateTime.UtcNow.AddDays(14)
        });

        return new AuthResult(newAccess, newRefreshValue);
    }

    public async Task LogoutAsync(Guid userId)
    {
        await _refreshRepo.RevokeByUserIdAsync(userId);
    }
}
