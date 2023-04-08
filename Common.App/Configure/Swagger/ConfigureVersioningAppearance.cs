using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.App.Configure.Swagger;

public class ConfigureVersioningAppearance : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly ILogger<ConfigureVersioningAppearance> _logger;

    public ConfigureVersioningAppearance(IApiVersionDescriptionProvider provider, ILogger<ConfigureVersioningAppearance> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var groupNameParts = description.GroupName.Split('_');

            if (groupNameParts.Length != 2)
                _logger.LogWarning("Invalid group name '{}'", description.GroupName);

            var version = groupNameParts[0];
            var packageName = groupNameParts.Length > 1 ? groupNameParts[1] : "Uncategorized";

            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = $"An definition of package {packageName}",
                Version = version,
            });
        }

        options.OperationFilter<RemoveVersionFromParameter>();
        options.DocumentFilter<SubstituteExactVersionInPath>();
    }
}