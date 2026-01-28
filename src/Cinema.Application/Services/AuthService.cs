using Cinema.Domain.Entities;
using Cinema.Domain.Interfaces;

namespace Cinema.Application.Services;

public sealed class AuthService(IUserRepository userRepo)
{
    private readonly IUserRepository _userRepo = userRepo;

    public async Task<User> ValidateUserAsync(string login, string password)
    {
        var user = await _userRepo.GetByLoginAsync(login)
            ?? throw new Exception("Invalid login or password");
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Invalid login or password");
        return user;
    }
}
