using Microsoft.AspNetCore.Authorization;

namespace Common.Infra.Auth.Policies;

public class AuthorizeHasClaimAttribute : AuthorizeAttribute
{
    public AuthorizeHasClaimAttribute(string claimType, params string[] validValues)
        : base(HasClaimPolicy.Name(claimType, validValues))
    {
    }

    public AuthorizeHasClaimAttribute(string claimType, params CommonManageTargets[] validValues) : base(
        HasClaimPolicy.Name(
            claimType,
            validValues.Select(value =>
                Enum.GetName(value)
                ?? throw new ArgumentException($"Argument {nameof(validValues)} contained invalid enum value"))))
    {
    }
}