using Cinema.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cinema.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        services.AddScoped<AuthService>();
        services.AddScoped<AdminUserService>();
        services.AddScoped<SessionService>();
        services.AddScoped<BookingService>();
        return services;
    }
}
