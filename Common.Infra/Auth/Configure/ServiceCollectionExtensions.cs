using Common.App.Services;
using Common.Infra.Auth.Policies;
using Common.Infra.Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Infra.Auth.Configure;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds default authorization services from <see cref="Common"/> package 
    /// </summary>
    /// <returns><see cref="CommonAuthorizationBuilder"/> which can be used to add more specific services</returns>
    public static CommonAuthorizationBuilder AddCommonAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(InRestaurantPolicy.Name, InRestaurantPolicy.Instanse);
        });

        services.AddSingleton<IAuthorizationHandler, PersonalDataHandler>();

        services.AddTransient<IAuthorizationHandlerProvider, OrderingAuthorizationHandlerProvider>();

        services.AddSingleton<IAuthorizationPolicyProvider, HasClaimPolicyProvider>();

        return new(services);
    }

    /// <summary>
    /// Adds Jwt authentication, expects <see cref="ITokenValidationParametersProvider"/> service implemented
    /// </summary>
    /// <returns><see cref="AuthenticationBuilder"/> so you can chain and modify authentication</returns>
    public static AuthenticationBuilder AddCommonJwtBearerAuth(this IServiceCollection services,
        Action<JwtBearerOptions>? configureOptions = null)
    {
        services.AddTransient<IConfigureNamedOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
        services.AddTransient<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

        var authenticationBuilder = services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        return configureOptions == null
            ? authenticationBuilder.AddJwtBearer()
            : authenticationBuilder.AddJwtBearer(configureOptions);
    }
}