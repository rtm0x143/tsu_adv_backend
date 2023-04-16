using Microsoft.AspNetCore.Authorization;

namespace Common.Infra.Auth.Policies;

public class AuthorizeGrantRoleAttribute : AuthorizeAttribute
{
    public AuthorizeGrantRoleAttribute(CommonRoles role) : base(GrantRolePolicy.Name(role))
    {
    }

    public AuthorizeGrantRoleAttribute(string roleName) : base(GrantRolePolicy.Name(roleName))
    {
    }
}