using Cinema.Domain.Interfaces;
using Cinema.Infrastructure.Data;
using Cinema.Infrastructure.Repositories;
using Cinema.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cinema.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(new DbConnectionFactory(connectionString));
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddScoped<ICacheVersionService, RedisCacheVersionService>();
        return services;
    }
}
