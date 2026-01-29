using Cinema.Domain.DTOs;
using Cinema.Domain.Interfaces;

namespace Cinema.Application.Services;

public class AdminUserService(IUserRepository repo)
{
    private readonly IUserRepository _repo = repo;

    public async Task<PagedResult<UserListItem>> GetUsersAsync(
        int page,
        int pageSize)
    {
        var (users, total) = await _repo.GetPagedAsync(page, pageSize);

        return new PagedResult<UserListItem>
        {
            Items = users,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }
}
