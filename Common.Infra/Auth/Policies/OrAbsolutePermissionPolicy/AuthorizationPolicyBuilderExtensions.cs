using Microsoft.AspNetCore.Authorization;

namespace Common.Infra.Auth.Policies;

public static class AuthorizationPolicyBuilderExtensions
{
    /// <summary>
    /// Adds <see cref="OrAbsolutePrivilegeRequirement"/>.
    /// </summary>
    public static AuthorizationPolicyBuilder OrAbsolutePrivilege(this AuthorizationPolicyBuilder builder,
        Action<AuthorizationPolicyBuilder> buildAbsolutePrivilegePolicy)
    {
        var absolutePrivilegeBuilder = new AuthorizationPolicyBuilder();
        buildAbsolutePrivilegePolicy.Invoke(absolutePrivilegeBuilder);

        builder.Combine(absolutePrivilegeBuilder.Build());
        builder.Requirements.Add(new OrAbsolutePrivilegeRequirement(absolutePrivilegeBuilder.Requirements));
        return builder;
    }
}