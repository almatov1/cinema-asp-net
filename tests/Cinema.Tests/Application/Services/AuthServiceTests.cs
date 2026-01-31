using Moq;
using FluentAssertions;
using Cinema.Domain.Interfaces;
using Cinema.Application.Services;
using Cinema.Domain.Entities;

namespace Cinema.Tests.Application.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly Mock<IRefreshTokenRepository> _refreshRepo = new();
    private readonly Mock<IJwtTokenService> _jwtService = new();
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authService = new AuthService(
            _userRepo.Object,
            _refreshRepo.Object,
            _jwtService.Object
        );
    }

    private static User CreateTestUser(string password = "Password123")
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Login = "testuser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };
    }

    // ===================== LoginAsync =====================
    [Fact]
    public async Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreValid()
    {
        var user = CreateTestUser();

        _userRepo.Setup(x => x.GetByLoginAsync("testuser"))
                 .ReturnsAsync(user);
        _jwtService.Setup(x => x.GenerateToken(user))
                   .Returns("access-token");
        _jwtService.Setup(x => x.GenerateRefreshToken())
                   .Returns("refresh-token");

        var result = await _authService.LoginAsync("testuser", "Password123");

        result.AccessToken.Should().Be("access-token");
        result.RefreshToken.Should().Be("refresh-token");

        _refreshRepo.Verify(x => x.CreateAsync(It.Is<RefreshToken>(
            t => t.UserId == user.Id && t.Token == "refresh-token")), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenUserNotFound()
    {
        _userRepo.Setup(x => x.GetByLoginAsync("wrong"))
                 .ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _authService.LoginAsync("wrong", "Password123"));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenPasswordInvalid()
    {
        var user = CreateTestUser("correct-password");
        _userRepo.Setup(x => x.GetByLoginAsync("testuser"))
                 .ReturnsAsync(user);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _authService.LoginAsync("testuser", "wrong-password"));
    }

    // ===================== RefreshAsync =====================
    [Fact]
    public async Task RefreshAsync_ShouldReturnNewTokens_WhenTokenValid()
    {
        var user = CreateTestUser();
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = "old-refresh",
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        _refreshRepo.Setup(x => x.GetAsync("old-refresh"))
                    .ReturnsAsync(refreshToken);
        _userRepo.Setup(x => x.GetByIdAsync(user.Id))
                 .ReturnsAsync(user);
        _jwtService.Setup(x => x.GenerateToken(user))
                   .Returns("new-access");
        _jwtService.Setup(x => x.GenerateRefreshToken())
                   .Returns("new-refresh");

        var result = await _authService.RefreshAsync("old-refresh");

        result.AccessToken.Should().Be("new-access");
        result.RefreshToken.Should().Be("new-refresh");

        _refreshRepo.Verify(x => x.RevokeAsync(refreshToken.Id), Times.Once);
        _refreshRepo.Verify(x => x.CreateAsync(It.Is<RefreshToken>(
            t => t.UserId == user.Id && t.Token == "new-refresh")), Times.Once);
    }

    [Fact]
    public async Task RefreshAsync_ShouldThrow_WhenTokenNotFound()
    {
        _refreshRepo.Setup(x => x.GetAsync("wrong-token"))
                    .ReturnsAsync((RefreshToken?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _authService.RefreshAsync("wrong-token"));
    }

    [Fact]
    public async Task RefreshAsync_ShouldThrow_WhenTokenExpired()
    {
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Token = "expired-token",
            ExpiresAt = DateTime.UtcNow.AddSeconds(-1)
        };

        _refreshRepo.Setup(x => x.GetAsync("expired-token"))
                    .ReturnsAsync(refreshToken);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _authService.RefreshAsync("expired-token"));
    }

    // ===================== LogoutAsync =====================
    [Fact]
    public async Task LogoutAsync_ShouldRevokeAllUserTokens()
    {
        var userId = Guid.NewGuid();

        await _authService.LogoutAsync(userId);

        _refreshRepo.Verify(x => x.RevokeByUserIdAsync(userId), Times.Once);
    }
}
