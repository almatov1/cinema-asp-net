using Cinema.Domain.DTOs;
using Cinema.Domain.Interfaces;

namespace Cinema.Application.Services;

public class AdminUserService(IUserRepository repo)
{
    private readonly IUserRepository _repo = repo;

    public async Task<PagedResult<UserListItemDto>> GetUsersAsync(
        int page,
        int pageSize)
    {
        var (users, total) = await _repo.GetPagedAsync(page, pageSize);

        return new PagedResult<UserListItemDto>
        {
            Items = users,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }
}
