using Auth.Infra.Auth.Policies;
using Common.Infra.Auth.Configure;
using Microsoft.AspNetCore.Authorization;

namespace Auth.Infra.Auth;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, GrantInRestaurantHandler>()
            .AddCommonAuthorization()
            .AddGeneralPolicyProvider();

        return services;
    }
}