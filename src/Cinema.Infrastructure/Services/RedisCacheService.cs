using System.Text.Json;
using Cinema.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Cinema.Infrastructure.Services;

public sealed class RedisCacheService(IDistributedCache cache)
    : ICacheService
{
    private readonly IDistributedCache _cache = cache;

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await _cache.GetStringAsync(key);

        return json is null
            ? default
            : JsonSerializer.Deserialize<T>(json);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan ttl)
    {
        var json = JsonSerializer.Serialize(value);

        await _cache.SetStringAsync(
            key,
            json,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl
            });
    }

    public Task RemoveAsync(string key)
        => _cache.RemoveAsync(key);
}
