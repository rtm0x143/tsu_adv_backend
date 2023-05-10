using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infra.Auth.Services;

public class CommonPolicyProviderConfiguration
{
    public Type? DefaultPolicyProviderType { get; private set; }
    public Type? FallbackPolicyProviderType { get; private set; }

    public IReadOnlyCollection<Type> PolicyProviderTypes => _policyProviderTypes;
    private readonly HashSet<Type> _policyProviderTypes = new();
    private readonly IServiceCollection _services;

    internal CommonPolicyProviderConfiguration(IServiceCollection services) => _services = services;

    public CommonPolicyProviderConfiguration AddPolicyProvider(Type type)
    {
        _services.AddSingleton(type);
        _policyProviderTypes.Add(type);
        return this;
    }

    /// <typeparam name="TPolicyProvider"><see cref="IAuthorizationPolicyProvider.GetPolicyAsync"/> should return null if handles unknown policy name</typeparam>
    public CommonPolicyProviderConfiguration AddPolicyProvider<TPolicyProvider>()
        where TPolicyProvider : class, IAuthorizationPolicyProvider
        => AddPolicyProvider(typeof(TPolicyProvider));

    public CommonPolicyProviderConfiguration AddDefaultPolicyProvider(Type type)
    {
        _services.AddSingleton(type);
        DefaultPolicyProviderType = type;
        return this;
    }

    public CommonPolicyProviderConfiguration AddDefaultPolicyProvider<TPolicyProvider>()
        where TPolicyProvider : class, IAuthorizationPolicyProvider
        => AddDefaultPolicyProvider(typeof(TPolicyProvider));

    public CommonPolicyProviderConfiguration AddFallbackPolicyProvider(Type type)
    {
        _services.AddSingleton(type);
        FallbackPolicyProviderType = type;
        return this;
    }

    public CommonPolicyProviderConfiguration AddFallbackPolicyProvider<TPolicyProvider>()
        where TPolicyProvider : class, IAuthorizationPolicyProvider
        => AddFallbackPolicyProvider(typeof(TPolicyProvider));
}