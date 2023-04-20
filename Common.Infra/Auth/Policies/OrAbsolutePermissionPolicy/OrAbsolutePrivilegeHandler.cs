using Microsoft.AspNetCore.Authorization;

namespace Common.Infra.Auth.Policies;

public class OrAbsolutePrivilegeHandler : AuthorizationHandler<IOrAbsolutePrivilegeRequirement> 
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        IOrAbsolutePrivilegeRequirement requirement)
    {
        context.Succeed(requirement);
        if (!requirement.IsFits(context.User)) return Task.CompletedTask;

        foreach (var pendingRequirement in context.PendingRequirements) context.Succeed(pendingRequirement);
        return Task.CompletedTask;
    }
}