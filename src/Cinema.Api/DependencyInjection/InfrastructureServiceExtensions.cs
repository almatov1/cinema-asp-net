using Cinema.Api.Services;

namespace Cinema.Api.DependencyInjection;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Services
        services.AddSingleton<JwtTokenService>();
        return services;
    }
}
