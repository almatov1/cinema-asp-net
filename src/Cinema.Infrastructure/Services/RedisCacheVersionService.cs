using Cinema.Domain.Interfaces;
using StackExchange.Redis;

namespace Cinema.Infrastructure.Services;

public sealed class RedisCacheVersionService(
    IConnectionMultiplexer redis)
    : ICacheVersionService
{
    private readonly IDatabase _db = redis.GetDatabase();

    public async Task<long> GetVersionAsync(string key)
    {
        var value = await _db.StringGetAsync(key);

        return value.HasValue
            ? (long)value
            : 1;
    }

    public async Task IncrementAsync(string key)
    {
        await _db.StringIncrementAsync(key);
    }
}
