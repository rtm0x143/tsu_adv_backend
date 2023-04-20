using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Common.Infra.Auth.Policies;

/// <inheritdoc cref="IOrAbsolutePrivilegeRequirement"/>
public class OrHasClaimRequirement : ClaimsAuthorizationRequirement, IOrAbsolutePrivilegeRequirement
{
    public OrHasClaimRequirement(string claimType, IEnumerable<string>? allowedValues) : base(claimType, allowedValues)
    {
    }

    public bool IsFits(ClaimsPrincipal principal) => 
        principal.FindFirstValue(ClaimType) is string value && (AllowedValues == null || AllowedValues.Contains(value));
}