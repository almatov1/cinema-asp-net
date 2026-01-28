using Cinema.Domain.Entities;

namespace Cinema.Domain.Interfaces;

public interface IUserRepository
{
    Task<Guid> CreateAsync(User user);
    Task<User?> GetByLoginAsync(string login);
}
