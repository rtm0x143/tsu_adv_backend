using Common.Api.jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Api.Configure.Jwt;

public static class JwtBearerConfigurationExtensions
{
    /// <summary>
    /// Adds Jwt authentication, singleton <see cref="IJwtValidator"/> service and also configures <see cref="IOptions{T}"/> for <see cref="JwtConfigurationProperties"/>  
    /// </summary>
    /// <returns><see cref="AuthenticationBuilder"/> so you can chain and modify authentication</returns>
    public static AuthenticationBuilder AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfigurationProperties>(
            options => options.ReadConfiguration(configuration));
        
        services.AddSingleton<IJwtValidator, JwtValidator>();
        services.AddTransient<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
        return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
    }
}