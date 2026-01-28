using Cinema.Domain.Entities;
using Cinema.Domain.Interfaces;

namespace Cinema.Application.Services;

public class UserService(IUserRepository repo)
{
    private readonly IUserRepository _repo = repo;

    public async Task<User> CreateUserAsync(string login, string password)
    {
        var exists = await _repo.GetByLoginAsync(login);
        if (exists != null)
            throw new Exception("User already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = login,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            CreatedAt = DateTime.UtcNow
        };

        await _repo.CreateAsync(user);
        return user;
    }
}
