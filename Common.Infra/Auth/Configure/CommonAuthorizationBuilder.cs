using Common.Infra.Auth.Policies;
using Common.Infra.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infra.Auth.Configure;

public class CommonAuthorizationBuilder
{
    private readonly IServiceCollection _services;
    internal CommonAuthorizationBuilder(IServiceCollection services) => _services = services;

    /// <summary>
    /// Adds <see cref="CommonPolicyProvider"/> and applies already registered <see cref="IAuthorizationPolicyProvider"/>s to it 
    /// </summary>
    /// <param name="configure">Action to apply <see cref="IAuthorizationPolicyProvider"/>s to <see cref="CommonPolicyProvider"/></param>
    public CommonAuthorizationBuilder AddGeneralPolicyProvider(
        Action<CommonPolicyProviderConfiguration>? configure = null)
    {
        var config = new CommonPolicyProviderConfiguration(_services);
        configure?.Invoke(config);

        foreach (var registeredPolicyProvider in _services.Where(
                     desc => desc.ImplementationType == typeof(IAuthorizationPolicyProvider)))
        {
            _services.Remove(registeredPolicyProvider);
            if (registeredPolicyProvider.ImplementationType != null)
                config.AddPolicyProvider(registeredPolicyProvider.ImplementationType);
        }

        var factory = new CommonPolicyProviderFactory(config);
        _services.AddSingleton<IAuthorizationPolicyProvider, CommonPolicyProvider>(factory.Create);

        return this;
    }

    /// <summary>
    /// Adds <see cref="OrAbsolutePrivilegeHandler"/>, <see cref="OrderingAuthorizationHandlerProvider"/>
    /// </summary>
    /// <remarks> <see cref="IAuthorizationHandlerProvider"/> should not be overriden </remarks>
    public CommonAuthorizationBuilder AddAbsolutePrivilegeRequirements()
    {
        _services.AddTransient<IAuthorizationHandlerProvider, OrderingAuthorizationHandlerProvider>();
        _services.AddSingleton<IAuthorizationHandler, OrAbsolutePrivilegeHandler>();

        return this;
    }
}