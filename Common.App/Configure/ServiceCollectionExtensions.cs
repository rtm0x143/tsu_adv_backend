using Asp.Versioning;
using Common.App.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Common.App.Configure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommonAppServices(this IServiceCollection services)
    {
        return services.AddSingleton<IExceptionsDescriber, CommonExceptionsDescriber>();
    }

    public static IApiVersioningBuilder AddCommonVersioning(this IServiceCollection services,
        int defaultMajorVersion = 1, int defaultMinorVersion = 0)
    {
        return services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(defaultMajorVersion, defaultMinorVersion);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(setup =>
            {
                setup.FormatGroupName = (groupName, apiVersion) => $"{apiVersion}_{groupName}";
                // ReSharper disable once StringLiteralTypo
                setup.GroupNameFormat = "'v'VVV";
            });
    }
}