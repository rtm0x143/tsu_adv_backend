using Common.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infra.Auth.Configure;

public static class CommonAuthExtensions
{
    public static CommonPolicyProviderBuilder AddCommonPolicyProvider(this IServiceCollection services) =>
        new(services.AddSingleton<IAuthorizationPolicyProvider, CommonPolicyProvider>());
}