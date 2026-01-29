using Cinema.Domain.DTOs;
using Cinema.Domain.Entities;

namespace Cinema.Domain.Interfaces;

public interface ISessionRepository
{
    Task<Guid> CreateAsync(Session session);
    Task<(IEnumerable<SessionListItem> Sessions, int Total)> GetPagedAsync(int page, int pageSize);
}
