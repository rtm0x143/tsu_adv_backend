using Common.App.Services.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Infra.Services.Jwt;

public static class JwtServicesExtensions
{
    /// <summary>
    /// Adds singleton implementation of <see cref="IJwtValidator"/> and <see cref="ITokenValidationParametersProvider"/>
    /// and also configures <see cref="IOptions{TOptions}"/> for <see cref="JwtConfigurationProperties"/>
    /// </summary>
    /// <returns>given <paramref name="services"/> parameter</returns>
    public static IServiceCollection AddCommonJwtServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConfigureOptions<JwtValidationOptions>, ConfigureJwtValidationOptions>();
        services.AddSingleton<IValidateOptions<JwtValidationOptions>, ValidateJwtValidationOptions>();
        
        services.AddSingleton<ITokenValidationParametersProvider, JwtValidatorService>();
        services.AddSingleton<IJwtValidator, JwtValidatorService>();
        return services;
    }
}
