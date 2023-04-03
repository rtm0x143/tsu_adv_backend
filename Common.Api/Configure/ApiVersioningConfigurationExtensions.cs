using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Api.Configure;

public static class ApiVersioningConfigurationExtensions
{
    public static IApiVersioningBuilder AddCommonVersioning(this IServiceCollection services)
    {
        return services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
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