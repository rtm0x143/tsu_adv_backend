using Auth.Features.Auth.Common;
using Auth.Features.Common;
using Auth.Infra.Services.Jwt;
using Auth.Infra.Services.Refresh;
using Common.Infra.Services.Jwt;
using Microsoft.Extensions.Options;

namespace Auth.Infra.Services;

public static class ConfigureInfraServicesExtension
{
    /// <summary>
    /// Adds to <paramref name="services"/> infrastructure services,
    /// also applies <see cref="ServiceCollectionExtensions.AddCommonJwtServices"/> 
    /// </summary>
    /// <returns><paramref name="services"/></returns>
    public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommonJwtServices(configuration);
        
        services.AddSingleton<IConfigureOptions<JwtConfigurationOptions>, ConfigureJwtConfigurationOptions>();
        services.AddSingleton<IValidateOptions<JwtConfigurationOptions>, ValidateJwtConfigurationOptions>();
        
        services.AddSingleton<IJwtGenerator, JwtGenerator>();
        
        services.Configure<RefreshTokenConfigurationProperties>(
            configuration.GetSection(RefreshTokenConfigurationProperties.ConfigurationSection));

        return services.AddScoped<IRefreshTokenHandler, RefreshTokenHandler>();
    }
}