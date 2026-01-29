using Cinema.Domain.Entities;
using Cinema.Domain.Interfaces;

namespace Cinema.Application.Services;

public class SessionService(ISessionRepository repo)
{
    private readonly ISessionRepository _repo = repo;

    public async Task<Session> CreateSessionAsync(string movieTitle, DateTime dateTime)
    {
        var session = new Session
        {
            Id = Guid.NewGuid(),
            MovieTitle = movieTitle,
            DateTime = dateTime,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.CreateAsync(session);
        return session;
    }
}
