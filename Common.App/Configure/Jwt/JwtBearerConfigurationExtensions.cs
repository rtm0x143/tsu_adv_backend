using Common.App.Services.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.App.Configure.Jwt;

public static class JwtBearerConfigurationExtensions
{
    /// <summary>
    /// Adds Jwt authentication, expects <see cref="ITokenValidationParametersProvider"/> service implemented
    /// </summary>
    /// <returns><see cref="AuthenticationBuilder"/> so you can chain and modify authentication</returns>
    public static AuthenticationBuilder AddCommonJwtBearerAuth(this IServiceCollection services)
    {
        services.AddTransient<IConfigureNamedOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
        services.AddTransient<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

        return services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                // options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();
    }
}