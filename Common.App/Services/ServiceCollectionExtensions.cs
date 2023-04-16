using Common.App.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Common.App.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommonAppServices(this IServiceCollection services)
    {
        return services.AddSingleton<IExceptionsDescriber, CommonExceptionsDescriber>();
    }
}