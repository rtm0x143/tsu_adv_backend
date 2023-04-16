using System.Reflection;
using Auth.Infra.Auth;
using Auth.Infra.Data.Configuration;
using Auth.Infra.Data.SeedData;
using Auth.Infra.Services;
using Common.App.Configure;
using Common.App.Configure.Jwt;
using Common.App.Configure.Swagger;
using Common.App.RequestHandlers;
using Common.App.Services;
using Common.Infra.Auth.Configure;
using Common.Infra.Services.SMTP;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddCommonVersioning();
builder.Services.AddCommonSwagger();

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddInfraServices(builder.Configuration)
    .AddCommonSmtpServices(builder.Configuration);

builder.Services.AddCommonJwtBearerAuth();
builder.Services.AddAuthAuthorization()
    .AddCommonPolicyProvider()
    .AddCommonPolicies();

builder.Services.AddUseCasesFrom(Assembly.GetExecutingAssembly())
    .AddCommonAppServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCommonSwagger();
    await app.Services.SeedDevelopmentData();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();