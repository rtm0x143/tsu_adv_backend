using Auth.Features.Auth.Common;
using Auth.Features.Common;
using Auth.Infra.Services.Refresh;
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
        
        services.Configure<RefreshTokenConfigurationProperties>(
            configuration.GetSection(RefreshTokenConfigurationProperties.ConfigurationSection));

        return services.AddScoped<IRefreshTokenHandler, RefreshTokenHandler>();
    }
}