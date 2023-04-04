using Common.Api.jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Api.Configure.Jwt;

public static class JwtBearerConfigurationExtensions
{
    /// <summary>
    /// Adds Jwt authentication, expects <see cref="ITokenValidationParametersProvider"/> service implemented
    /// </summary>
    /// <returns><see cref="AuthenticationBuilder"/> so you can chain and modify authentication</returns>
    public static AuthenticationBuilder AddCommonJwtBearerAuth(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
        return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
    }
}