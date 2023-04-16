using Microsoft.AspNetCore.Authorization;

namespace Common.Infra.Auth.Policies;

public static class GrantRolePolicy
{
    internal const string PolicyPrefix = "Grant";
    public static string Name(string roleName) => PolicyPrefix + roleName;
    public static string Name(CommonRoles role) => PolicyPrefix + Enum.GetName(role);

    public static AuthorizationPolicy Create(string roleName) =>
        new AuthorizationPolicyBuilder()
            .RequireClaim(CommonClaimTypes.Grant, roleName)
            .Build();

    public static AuthorizationPolicy Create(CommonRoles role) => Create(Enum.GetName(role)!);
}