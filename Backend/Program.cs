using System.Reflection;
using Backend.Common.Dtos;
using Backend.Features.Dish;
using Backend.Infra.Data;
using Backend.Infra.Messaging;
using Common.App.Configure;
using Common.App.Configure.Swagger;
using Common.App.RequestHandlers;
using Common.Infra.Auth.Configure;
using Common.Infra.Services.Jwt;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
});

builder.Services.AddProblemDetails();
builder.Services.AddRouting(configure => configure.LowercaseUrls = true);

builder.Services.AddCommonVersioning();
builder.Services.AddCommonSwagger()
    .AddSwaggerGen(options =>
    {
        options.MapType<OrderNumber>(() => new OpenApiSchema
            { Type = "string", Format = "string", Title = "Base32 crockford encoded 64-bit integer" });
    });

builder.Services.AddBackendDbContexts(builder.Configuration);

builder.Services.AddCommonJwtServices(builder.Configuration)
    .AddCommonJwtBearerAuth();

builder.Services.AddCommonAuthorization()
    .AddGeneralPolicyProvider()
    .AddAbsolutePrivilegeRequirements();

builder.Services.AddCommonAppServices()
    .AddRequestHandlersFrom(Assembly.GetExecutingAssembly());

builder.Services.AddDishFeatureServices();

builder.Host.ConfigureMessageBus();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCommonSwagger();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();