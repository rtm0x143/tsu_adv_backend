using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Common.App.Configure.Swagger;

public static class SwaggerConfigurationExtensions
{
    public static IServiceCollection AddCommonSwagger(this IServiceCollection services)
    {   
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
        services.AddTransient<IConfigureOptions<SwaggerUIOptions>, ConfigureSwaggerUIOptions>();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureVersioningAppearance>();
        
        return services.AddSwaggerGen();
    }

    public static IApplicationBuilder UseCommonSwagger(this IApplicationBuilder builder)
    {
        builder.UseSwagger(setup =>
        {
            setup.RouteTemplate = "swagger/{documentName}.json";
        });

        return builder.UseSwaggerUI();
    }
}