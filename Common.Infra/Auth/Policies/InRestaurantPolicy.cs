using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Common.Infra.Auth.Policies;

public static class InRestaurantPolicy
{
    public const string Name = "InRestaurant";

    /// <summary>
    /// Only check if user belongs to some restaurant
    /// </summary>
    public static AuthorizationPolicy Instanse { get; } = new AuthorizationPolicyBuilder()
        .RequireClaim(CommonClaimTypes.Restaurant)
        .Build();

    /// <summary>
    /// Requires user to belong to restaurant with <paramref name="restaurantId"/>
    /// </summary>
    public static IAuthorizationRequirement CreateRequirement(Guid restaurantId) =>
        new ClaimsAuthorizationRequirement(CommonClaimTypes.Restaurant, new[] { restaurantId.ToString() });
}