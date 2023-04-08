using System.Reflection;
using Common.App.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Common.App.UseCases;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all <see cref="IUseCase{TRequest,TResult}"/>, <see cref="IAsyncUseCase{TRequest,TResult}"/>
    /// and any interface annotated with <see cref="UseCaseInterfaceAttribute"/>
    /// implementations and that services itself to <see cref="IServiceCollection"/>
    /// </summary>
    /// <param name="services">Target <see cref="IServiceCollection"/></param>
    /// <param name="assembly">An <see cref="Assembly"/> to search for implementations</param>
    /// <returns>Given <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddUseCasesFrom(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(t => !t.IsAbstract))
        {
            var useCaseInterfaces = type.GetInterfaces()
                .Where(i => i.Name == typeof(IUseCase<,>).Name
                            || i.Name == typeof(IAsyncUseCase<,>).Name
                            || i.CustomAttributes.FirstOrDefault(a =>
                                a.AttributeType == typeof(UseCaseInterfaceAttribute)) != null)
                .ToArray();
            if (useCaseInterfaces.Length == 0) continue;

            services.AddScoped(type);
            foreach (var @interface in useCaseInterfaces)
                services.AddScoped(@interface, type);
        }

        return services;
    }
}