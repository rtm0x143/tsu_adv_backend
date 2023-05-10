using Microsoft.AspNetCore.Authorization;

namespace Common.Infra.Auth.Services;

public sealed class CommonPolicyProvider : IAuthorizationPolicyProvider
{
    public required IAuthorizationPolicyProvider DefaultPolicyProvider { get; set; }
    public required IAuthorizationPolicyProvider FallbackPolicyProvider { get; set; }

    /// <summary>
    /// Collection <see cref="IAuthorizationPolicyProvider"/> which <see cref="IAuthorizationPolicyProvider.GetPolicyAsync"/> should return null if handles unknown policy name 
    /// </summary>
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