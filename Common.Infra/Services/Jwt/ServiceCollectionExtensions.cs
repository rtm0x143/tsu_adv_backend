using Common.App.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Infra.Services.Jwt;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds singleton implementation of <see cref="ITokenValidator"/> and <see cref="ITokenValidationParametersProvider"/>
    /// and also configures <see cref="IOptions{TOptions}"/> for <see cref="JwtValidationOptions"/>
    /// </summary>
    /// <returns>given <paramref name="services"/> parameter</returns>
    public static IServiceCollection AddCommonJwtServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConfigureOptions<JwtValidationOptions>, ConfigureJwtValidationOptions>();
        services.AddSingleton<IValidateOptions<JwtValidationOptions>, ValidateJwtValidationOptions>();
        
        services.AddSingleton<ITokenValidationParametersProvider, TokenValidatorService>();
        services.AddSingleton<ITokenValidator, TokenValidatorService>();
        return services;
    }
}
