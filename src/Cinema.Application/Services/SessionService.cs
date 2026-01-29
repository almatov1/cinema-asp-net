using Cinema.Domain.DTOs;
using Cinema.Domain.Entities;
using Cinema.Domain.Interfaces;

namespace Cinema.Application.Services;

public class SessionService(
    ISessionRepository repo,
    ICacheService cache,
    ICacheVersionService versionService)
{
    private readonly ISessionRepository _repo = repo;
    private readonly ICacheService _cache = cache;
    private readonly ICacheVersionService _versionService = versionService;

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
        await _versionService.IncrementAsync("sessions:version");
        return session;
    }

    public async Task<PagedResult<SessionListItem>> GetSessionsAsync(
        int page,
        int pageSize)
    {
        const string versionKey = "sessions:version";

        var version =
            await _versionService.GetVersionAsync(versionKey);

        var cacheKey =
            $"sessions:v{version}:p{page}:s{pageSize}";

        var cached =
            await _cache
                .GetAsync<PagedResult<SessionListItem>>(cacheKey);

        if (cached is not null)
            return cached;

        var (sessions, total) = await _repo.GetPagedAsync(page, pageSize);
        var result = new PagedResult<SessionListItem>
        {
            Items = sessions,
            Total = total,
            Page = page,
            PageSize = pageSize
        };

        await _cache.SetAsync(
        cacheKey,
        result,
        TimeSpan.FromMinutes(2));

        return result;
    }
}
