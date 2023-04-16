using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Common.Infra.Auth.Policies;

public abstract class AuthorizationPolicyProviderBase : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _defaultAuthorizationPolicyProvider;

    protected AuthorizationPolicyProviderBase(IOptions<AuthorizationOptions> options) =>
        _defaultAuthorizationPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    
    public abstract Task<AuthorizationPolicy?> GetPolicyAsync(string policyName);

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
        _defaultAuthorizationPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
        _defaultAuthorizationPolicyProvider.GetFallbackPolicyAsync();
}