using Cinema.Domain.Entities;
using Cinema.Domain.Interfaces;

namespace Cinema.Application.Services;

public sealed class AuthService(IUserRepository userRepo)
{
    private readonly IUserRepository _userRepo = userRepo;
    private readonly string _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new Exception("JWT_SECRET not set");
    private readonly int _jwtExpiresMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES") ?? "60");

    public async Task<User> ValidateUserAsync(string login, string password)
    {
        var user = await _userRepo.GetByLoginAsync(login)
            ?? throw new Exception("Invalid login");
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Invalid password");
        return user;
    }
}
