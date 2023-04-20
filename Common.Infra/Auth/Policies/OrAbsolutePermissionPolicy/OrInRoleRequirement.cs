using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Common.Infra.Auth.Policies;

/// <inheritdoc cref="IOrAbsolutePrivilegeRequirement"/>
public class OrInRoleRequirement : RolesAuthorizationRequirement, IOrAbsolutePrivilegeRequirement
{
    public static readonly OrInRoleRequirement OrAdmin = new(new[] { Enum.GetName(CommonRoles.Admin)! });

    public OrInRoleRequirement(IEnumerable<string> allowedRoles) : base(allowedRoles)
    {
    }

    public bool IsFits(ClaimsPrincipal principal) => AllowedRoles.Any(principal.IsInRole);
}