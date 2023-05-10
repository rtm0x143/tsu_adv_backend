using Common.Infra.Auth;
using Common.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Features.Menu.Common;

public static class ManageMenuInRestaurantPolicy
{
    private static int _maxCacheSize;
    private static readonly Dictionary<Guid, AuthorizationPolicy> _cachedPolicies = new();

    public static int MaxCacheSize
    {
        get => _maxCacheSize;
        set => _cachedPolicies.TrimExcess(_maxCacheSize = value);
    }

    static ManageMenuInRestaurantPolicy() => MaxCacheSize = 1000;

    private static AuthorizationPolicy _create(Guid restaurantId) =>
        new AuthorizationPolicyBuilder()
            .RequireClaim(CommonClaimTypes.Manage, Enum.GetName(CommonManageTargets.Menu)!)
            .AddRequirements(InRestaurantPolicy.CreateRequirement(restaurantId))
            .OrAbsolutePrivilege(builder
                => builder.RequireRole(nameof(CommonRoles.Admin)))
            .Build();

    public static AuthorizationPolicy Create(Guid restaurantId)
    {
        if (!_cachedPolicies.TryGetValue(restaurantId, out var policy))
            _cachedPolicies.Add(restaurantId, policy = _create(restaurantId));
        return policy;
    }
}