using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Infra.Auth.Policies;

public class CommonPolicyProvider : IAuthorizationPolicyProvider
{
    internal static readonly List<Type> PolicyProviderTypes = new();
    internal static Type? DefaultPolicyProviderType = null;
    internal static Type? FallbackPolicyProviderType = null;

    private readonly IAuthorizationPolicyProvider _defaultPolicyProvider;
    private readonly IAuthorizationPolicyProvider _fallbackPolicyProvider;

    private readonly IAuthorizationPolicyProvider[] _policyProviders;

    public CommonPolicyProvider(IOptions<AuthorizationOptions> options, IServiceProvider serviceProvider)
    {
        _defaultPolicyProvider
            = (DefaultPolicyProviderType != null
                  ? serviceProvider.GetRequiredService(DefaultPolicyProviderType) as IAuthorizationPolicyProvider
                  : null)
              ?? new DefaultAuthorizationPolicyProvider(options);
        _fallbackPolicyProvider
            = (FallbackPolicyProviderType != null
                  ? serviceProvider.GetRequiredService(FallbackPolicyProviderType) as IAuthorizationPolicyProvider
                  : null)
              ?? new DefaultAuthorizationPolicyProvider(options);
        _policyProviders = PolicyProviderTypes.Select(serviceProvider.GetRequiredService)
            .Where(provider => provider is IAuthorizationPolicyProvider)
            .Cast<IAuthorizationPolicyProvider>()
            .ToArray();
    }

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        foreach (var provider in _policyProviders)
        {
            if (await provider.GetPolicyAsync(policyName) is AuthorizationPolicy policy) return policy;
        }

        return await _defaultPolicyProvider.GetPolicyAsync(policyName);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _defaultPolicyProvider.GetDefaultPolicyAsync();
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallbackPolicyProvider.GetFallbackPolicyAsync();
}