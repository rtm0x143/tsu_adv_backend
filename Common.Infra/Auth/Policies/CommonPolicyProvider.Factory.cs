using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Infra.Auth.Policies;

internal class CommonPolicyProviderFactory
{
    private readonly CommonPolicyProviderConfiguration _config;
    internal CommonPolicyProviderFactory(CommonPolicyProviderConfiguration configuration) => _config = configuration;

    internal CommonPolicyProvider Create(IServiceProvider services)
    {
        IAuthorizationPolicyProvider getPolicyProvider(Type policyProviderType)
        {
            if (services.GetRequiredService(policyProviderType) is IAuthorizationPolicyProvider provider)
                return provider;
            throw new InvalidCastException(
                $"Service of type {policyProviderType.FullName} in service provider doesn't implement {nameof(IAuthorizationPolicyProvider)}");
        }

        DefaultAuthorizationPolicyProvider? defaultPolicyProvider = null;
        if (_config.DefaultPolicyProviderType == null || _config.FallbackPolicyProviderType == null)
        {
            defaultPolicyProvider = new DefaultAuthorizationPolicyProvider(
                services.GetRequiredService<IOptions<AuthorizationOptions>>());
        }

        return new()
        {
            DefaultPolicyProvider = _config.DefaultPolicyProviderType == null
                ? defaultPolicyProvider!
                : getPolicyProvider(_config.DefaultPolicyProviderType),
            FallbackPolicyProvider = _config.FallbackPolicyProviderType == null
                ? defaultPolicyProvider!
                : getPolicyProvider(_config.FallbackPolicyProviderType),
            PolicyProviders = _config.PolicyProviderTypes.Select(getPolicyProvider).ToArray()
        };
    }
}