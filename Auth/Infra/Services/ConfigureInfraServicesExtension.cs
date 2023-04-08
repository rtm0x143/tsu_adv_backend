using Common.Infra.Services.Jwt;

namespace Auth.Infra.Services;

public static class ConfigureInfraServicesExtension
{
    /// <summary>
    /// Adds to <paramref name="services"/> infrastructure services,
    /// also applies <see cref="JwtServicesExtensions.AddCommonJwtServices"/> 
    /// </summary>
    /// <returns><paramref name="services"/></returns>
    public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommonJwtServices(configuration);
        services.AddSingleton<IJwtGenerator, JwtGenerator>();
        return services;
    }
}