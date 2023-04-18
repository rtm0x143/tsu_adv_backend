using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.App.Configure.Swagger;

internal class RemoveVersionFromParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters.SingleOrDefault(p => p.Name == "version") is OpenApiParameter parameter)
            operation.Parameters.Remove(parameter);
    }
}