using Cinema.Domain.Entities;

namespace Cinema.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task CreateAsync(RefreshToken token);
    Task<RefreshToken?> GetAsync(string token);
    Task RevokeAsync(Guid id);
    Task RevokeByUserIdAsync(Guid userId);
}
