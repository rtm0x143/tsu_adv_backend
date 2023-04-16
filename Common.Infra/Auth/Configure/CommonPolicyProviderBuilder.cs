using Common.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Infra.Auth.Configure;

public class CommonPolicyProviderBuilder
{
    private readonly IServiceCollection _services;

    internal CommonPolicyProviderBuilder(IServiceCollection services) => _services = services;

    public CommonPolicyProviderBuilder AddPolicyProvider<TPolicyProvider>()
        where TPolicyProvider : class, IAuthorizationPolicyProvider
    {
        _services.AddSingleton<TPolicyProvider>();
        CommonPolicyProvider.PolicyProviderTypes.Add(typeof(TPolicyProvider));
        return this;
    }

    public CommonPolicyProviderBuilder AddDefaultPolicyProvider<TPolicyProvider>()
        where TPolicyProvider : class, IAuthorizationPolicyProvider
    {
        _services.AddSingleton<TPolicyProvider>();
        CommonPolicyProvider.DefaultPolicyProviderType = typeof(TPolicyProvider);
        return this;
    }

    public CommonPolicyProviderBuilder AddFallbackPolicyProvider<TPolicyProvider>()
        where TPolicyProvider : class, IAuthorizationPolicyProvider
    {
        _services.AddSingleton<TPolicyProvider>();
        CommonPolicyProvider.FallbackPolicyProviderType = typeof(TPolicyProvider);
        return this;
    }

    public CommonPolicyProviderBuilder AddCommonPolicies()
    {
        _services.Configure<AuthorizationOptions>(options =>
        {
            options.AddPolicy(InRestaurantPolicy.Name, InRestaurantPolicy.Instanse);
        });
        return AddPolicyProvider<GrantRolePolicyProvider>();
    }
}