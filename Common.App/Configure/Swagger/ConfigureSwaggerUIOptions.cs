using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Common.App.Configure.Swagger;

// ReSharper disable once InconsistentNaming
public class ConfigureSwaggerUIOptions : IConfigureOptions<SwaggerUIOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerUIOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

    public void Configure(SwaggerUIOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"{description.GroupName}.json",
                description.GroupName.Replace('_', ' '));
        }
    }
}
