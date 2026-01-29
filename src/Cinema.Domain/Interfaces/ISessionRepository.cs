using Cinema.Domain.Entities;

namespace Cinema.Domain.Interfaces;

public interface ISessionRepository
{
    Task<Guid> CreateAsync(Session session);
}
