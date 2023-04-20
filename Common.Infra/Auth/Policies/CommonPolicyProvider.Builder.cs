using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infra.Auth.Policies;

public class CommonPolicyProviderConfiguration
{
    public required IServiceCollection Services { get; init; }
    public Type DefaultPolicyProviderType { get; private set; } = typeof(DefaultAuthorizationPolicyProvider);
    public Type FallbackPolicyProviderType { get; private set; } = typeof(DefaultAuthorizationPolicyProvider);

    public IReadOnlyCollection<Type> PolicyProviderTypes => _policyProviderTypes;
    private readonly HashSet<Type> _policyProviderTypes = new();

    internal CommonPolicyProviderConfiguration()
    {
    }

    public CommonPolicyProviderConfiguration AddPolicyProvider<TPolicyProvider>()
        where TPolicyProvider : class, IAuthorizationPolicyProvider
    {
        Services.AddSingleton<TPolicyProvider>();
        _policyProviderTypes.Add(typeof(TPolicyProvider));
        return this;
    }

    public CommonPolicyProviderConfiguration AddDefaultPolicyProvider<TPolicyProvider>()
        where TPolicyProvider : class, IAuthorizationPolicyProvider
    {
        Services.AddSingleton<TPolicyProvider>();
        DefaultPolicyProviderType = typeof(TPolicyProvider);
        return this;
    }

    public CommonPolicyProviderConfiguration AddFallbackPolicyProvider<TPolicyProvider>()
        where TPolicyProvider : class, IAuthorizationPolicyProvider
    {
        Services.AddSingleton<TPolicyProvider>();
        FallbackPolicyProviderType = typeof(TPolicyProvider);
        return this;
    }

    public CommonPolicyProviderConfiguration AddCommonPolicies()
    {
        Services.Configure<AuthorizationOptions>(options =>
        {
            options.AddPolicy(InRestaurantPolicy.Name, InRestaurantPolicy.Instanse);
        });
        return AddPolicyProvider<GrantRolePolicyProvider>();
    }
}