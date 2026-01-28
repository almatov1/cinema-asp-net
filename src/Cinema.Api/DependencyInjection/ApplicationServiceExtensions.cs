using Cinema.Application.Services;
using Cinema.Domain.Interfaces;
using Cinema.Infrastructure.Repositories;
using Cinema.Infrastructure.Data;

namespace Cinema.Api.DependencyInjection;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, string connectionString)
    {
        // DB
        services.AddSingleton(new DbConnectionFactory(connectionString));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Application Services
        services.AddScoped<UserService>();
        services.AddScoped<AuthService>();
        services.AddScoped<AdminUserService>();

        return services;
    }
}
