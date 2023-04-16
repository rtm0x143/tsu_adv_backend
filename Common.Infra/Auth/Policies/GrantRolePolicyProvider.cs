using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Common.Infra.Auth.Policies;

public class GrantRolePolicyProvider : AuthorizationPolicyProviderBase
{
    public GrantRolePolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }

    public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!policyName.StartsWith(GrantRolePolicy.PolicyPrefix))
            return Task.FromResult<AuthorizationPolicy?>(null);

        return Task.FromResult(GrantRolePolicy.Create(policyName[GrantRolePolicy.PolicyPrefix.Length..]))!;
    }
}