using Common.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infra.Auth.Configure;

public static class CommonAuthExtensions
{
    public static IServiceCollection AddCommonPolicyProvider(this IServiceCollection services,
        Action<CommonPolicyProviderConfiguration>? configure = null)
    {
        var config = new CommonPolicyProviderConfiguration { Services = services };
        configure?.Invoke(config);
        var factory = new CommonPolicyProviderFactory(config);
        return services.AddSingleton<IAuthorizationPolicyProvider, CommonPolicyProvider>(factory.Create);
    }
}