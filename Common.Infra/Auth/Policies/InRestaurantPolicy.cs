using Microsoft.AspNetCore.Authorization;

namespace Common.Infra.Auth.Policies;

public static class InRestaurantPolicy
{
    public const string Name = "InRestaurant";

    public static AuthorizationPolicy Instanse { get; } = new AuthorizationPolicyBuilder()
        .RequireClaim(CommonClaimTypes.Restaurant)
        .Build();
}