using Common.Domain.Utils;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Auth.Infra.Auth.Policies;

public record GrantInRestaurantRequirement(Guid Restaurant, string RoleName) : IAuthorizationRequirement
{
    public GrantInRestaurantRequirement(Guid restaurant, CommonRoles role) : this(restaurant, Enum.GetName(role)!)
    {
    }
}

public class GrantInRestaurantHandler : AuthorizationHandler<GrantInRestaurantRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        GrantInRestaurantRequirement requirement)
    {
        var grantClaims = context.User.FindAll(CommonClaimTypes.Grant).Select(c => c.Value);
        if (context.User.IsInRole(Enum.GetName(CommonRoles.Admin)!)
            || (grantClaims.Contains(requirement.RoleName)
                && context.User.TryFindFirstGuid(CommonClaimTypes.Restaurant, out var restIdClaim)
                && restIdClaim == requirement.Restaurant))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}