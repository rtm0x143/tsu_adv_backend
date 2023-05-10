using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Infra.Auth.Services;

internal class CommonPolicyProviderFactory
{
    private readonly CommonPolicyProviderConfiguration _config;
    internal CommonPolicyProviderFactory(CommonPolicyProviderConfiguration configuration) => _config = configuration;

    internal CommonPolicyProvider Create(IServiceProvider services)
    {
        IAuthorizationPolicyProvider GetPolicyProvider(Type policyProviderType)
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
                : GetPolicyProvider(_config.DefaultPolicyProviderType),
            FallbackPolicyProvider = _config.FallbackPolicyProviderType == null
                ? defaultPolicyProvider!
                : GetPolicyProvider(_config.FallbackPolicyProviderType),
            PolicyProviders = _config.PolicyProviderTypes.Select(GetPolicyProvider).ToArray()
        };
    }
}