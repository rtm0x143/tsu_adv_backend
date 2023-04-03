using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Api.Configure.Swagger;

internal class SubstituteExactVersionInPath : IDocumentFilter
{
    public void Apply(OpenApiDocument document, DocumentFilterContext context)
    {
        var substitutePaths = new OpenApiPaths();
        foreach (var (key, value) in document.Paths)
            substitutePaths.Add(key.Replace("v{version}", document.Info.Version), value);

        document.Paths = substitutePaths;
    }
}