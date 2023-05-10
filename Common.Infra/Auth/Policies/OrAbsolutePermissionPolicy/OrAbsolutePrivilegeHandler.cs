using Common.Infra.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Common.Infra.Auth.Policies;

public class OrAbsolutePrivilegeHandler : AuthorizationHandlerWithOrder<OrAbsolutePrivilegeRequirement>
{
    /// <summary>
    /// Marks all <paramref name="context"/>.<see cref="AuthorizationHandlerContext.PendingRequirements"/> as succeeded
    /// if <paramref name="requirement"/>.<see cref="OrAbsolutePrivilegeRequirement.AbsolutePrivilegeRequirements"/> succeeded
    /// </summary>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        OrAbsolutePrivilegeRequirement requirement)
    {
        var notSucceededAbsoluteRequirements =
            requirement.AbsolutePrivilegeRequirements.Intersect(context.PendingRequirements).ToArray();

        if (notSucceededAbsoluteRequirements.Length == 0)
        {
            foreach (var pendingRequirement in context.PendingRequirements) context.Succeed(pendingRequirement);
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        foreach (var absoluteRequirement in notSucceededAbsoluteRequirements) context.Succeed(absoluteRequirement);

        return Task.CompletedTask;
    }

    /// <summary>
    /// That handler want to be executed in the last order of requirement processing
    /// </summary>
    public override int OrderFactor => int.MaxValue;
}