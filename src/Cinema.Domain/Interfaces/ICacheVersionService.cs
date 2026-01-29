namespace Cinema.Domain.Interfaces;

public interface ICacheVersionService
{
    Task<long> GetVersionAsync(string key);

    Task IncrementAsync(string key);
}
