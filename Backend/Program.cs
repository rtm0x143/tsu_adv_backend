using System.Reflection;
using Backend.Controllers;
using Backend.Infra.Data;
using Common.App.Configure;
using Common.App.Configure.Jwt;
using Common.App.Configure.Swagger;
using Common.App.RequestHandlers;
using Common.App.Services;
using Common.Infra.Auth.Configure;
using Common.Infra.Services.Jwt;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddCommonVersioning();
builder.Services.AddCommonSwagger()
    .AddSwaggerGen(options =>
    {
        options.MapType<OrderNumber>(() => new OpenApiSchema
            { Type = "string", Format = "string", Title = "Base32 crockford encoded 64-bit integer" });
    });

builder.Services.AddBackendDbContext(builder.Configuration);

builder.Services.AddCommonJwtServices(builder.Configuration)
    .AddCommonJwtBearerAuth();

builder.Services.AddAuthenticationCore()
    .AddCommonPolicyProvider(configuration => configuration.AddCommonPolicies());

builder.Services.AddCommonAppServices()
    .AddRequestHandlersFrom(Assembly.GetExecutingAssembly());

builder.Services.AddRouting(configure => configure.LowercaseUrls = true);

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