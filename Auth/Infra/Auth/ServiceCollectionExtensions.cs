using Auth.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;

namespace Auth.Infra.Auth;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthAuthorization(this IServiceCollection services)
    {
        return services.AddSingleton<IAuthorizationHandler, GrantInRestaurantHandler>()
            .AddSingleton<IAuthorizationHandler, PersonalDataHandler>()
            .AddAuthorization();
    }
}