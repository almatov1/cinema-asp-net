using Cinema.Domain.DTOs;
using Cinema.Domain.Entities;

namespace Cinema.Domain.Interfaces;

public interface IUserRepository
{
    Task<Guid> CreateAsync(User user);
    Task<User?> GetByLoginAsync(string login);
    Task<User?> GetByIdAsync(Guid id);
    Task<(IEnumerable<UserListItemDto> Users, int Total)>
    GetPagedAsync(int page, int pageSize);
}
