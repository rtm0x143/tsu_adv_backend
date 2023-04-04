using Common.Api.jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Infra.Jwt;

public static class JwtServicesExtensions
{
    /// <summary>
    /// Adds singleton implementation of <see cref="IJwtValidator"/> and <see cref="ITokenValidationParametersProvider"/>
    /// and also configures <see cref="IOptions{TOptions}"/> for <see cref="JwtConfigurationProperties"/>
    /// </summary>
    /// <returns>given <paramref name="services"/> parameter</returns>
    public static IServiceCollection AddCommonJwtServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfigurationProperties>(
            options => options.ReadConfiguration(configuration));
        
        services.AddSingleton<ITokenValidationParametersProvider, JwtValidator>();
        services.AddSingleton<IJwtValidator, JwtValidator>();
        return services;
    }
}