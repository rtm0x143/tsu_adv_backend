using System.Reflection;
using Common.App.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Common.App.RequestHandlers;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all <see cref="IRequestHandler{TRequest,TResult}"/>
    /// and any interface annotated with <see cref="RequestHandlerInterfaceAttribute"/>
    /// implementations and that services itself to <see cref="IServiceCollection"/>
    /// </summary>
    /// <param name="services">Target <see cref="IServiceCollection"/></param>
    /// <param name="assembly">An <see cref="Assembly"/> to search for implementations</param>
    /// <returns>Given <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddRequestHandlersFrom(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(t => !t.IsAbstract))
        {
            var useCaseInterfaces = type.GetInterfaces()
                .Where(i => i.Name == typeof(IRequestHandler<,>).Name
                            || i.CustomAttributes.FirstOrDefault(a =>
                                a.AttributeType == typeof(RequestHandlerInterfaceAttribute)) != null)
                .ToArray();
            if (useCaseInterfaces.Length == 0) continue;

            services.AddScoped(type);
            foreach (var @interface in useCaseInterfaces)
                services.AddScoped(@interface, type);
        }

        return services;
    }
}