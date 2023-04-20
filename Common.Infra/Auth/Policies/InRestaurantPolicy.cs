using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Common.Infra.Auth.Policies;

public static class InRestaurantPolicy
{
    public const string Name = "InRestaurant";

    public static AuthorizationPolicy Instanse { get; } = new AuthorizationPolicyBuilder()
        .RequireClaim(CommonClaimTypes.Restaurant)
        .Build();

    public static IAuthorizationRequirement CreateRequirement(Guid restaurantId) =>
        new ClaimsAuthorizationRequirement(CommonClaimTypes.Restaurant, new[] { restaurantId.ToString() });
}