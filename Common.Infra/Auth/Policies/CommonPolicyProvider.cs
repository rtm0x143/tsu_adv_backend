using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Infra.Auth.Policies;

public sealed partial class CommonPolicyProvider : IAuthorizationPolicyProvider
{
    public required IAuthorizationPolicyProvider DefaultPolicyProvider { get; set; }
    public required IAuthorizationPolicyProvider FallbackPolicyProvider { get; set; }

    public ICollection<IAuthorizationPolicyProvider> PolicyProviders { get; init; } =
        new List<IAuthorizationPolicyProvider>();

    internal CommonPolicyProvider()
    {
    }

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        foreach (var provider in PolicyProviders)
        {
            if (await provider.GetPolicyAsync(policyName) is AuthorizationPolicy policy) return policy;
        }

        return await DefaultPolicyProvider.GetPolicyAsync(policyName);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => DefaultPolicyProvider.GetDefaultPolicyAsync();
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();
}