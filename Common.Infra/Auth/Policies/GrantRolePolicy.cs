namespace Common.Infra.Auth.Policies;

public static class GrantRolePolicy
{
    public static string Name(string roleName) => HasClaimPolicy.Name(CommonClaimTypes.Grant, roleName);

    public static string Name(CommonRoles role) => 
        Name(Enum.GetName(role) ?? throw new ArgumentException($"Argument {nameof(role)} had invalid enum value"));
}