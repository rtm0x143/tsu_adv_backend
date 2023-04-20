using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Common.Infra.Auth.Policies;

public class HasClaimPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public HasClaimPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }

    public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!HasClaimPolicy.TryParseName(policyName, out var claimType, out var validValues))
            return Task.FromResult<AuthorizationPolicy?>(null);

        var builder = new AuthorizationPolicyBuilder();
        if (validValues == null)
            builder.RequireClaim(claimType);
        else
            builder.RequireClaim(claimType, validValues);

        return Task.FromResult<AuthorizationPolicy?>(builder.Build());
    }
}