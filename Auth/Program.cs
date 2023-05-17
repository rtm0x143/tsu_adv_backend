using System.Reflection;
using Auth.Infra.Auth;
using Auth.Infra.Data.Configuration;
using Auth.Infra.Data.SeedData;
using Auth.Infra.Messaging;
using Auth.Infra.Services;
using Common.App.Configure;
using Common.App.Configure.Swagger;
using Common.App.RequestHandlers;
using Common.Infra.Auth.Configure;
using Common.Infra.Messaging;
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
builder.Services.AddAuthAuthorization();

builder.Services.AddRequestHandlersFrom(Assembly.GetExecutingAssembly())
    .AddCommonAppServices();

builder.Services.AddRouting(configure => configure.LowercaseUrls = true);

builder.Host.ConfigureMessageBus<AuthMessageBusConfiguration>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCommonSwagger();
#pragma warning disable CS4014
    app.Services.SeedDevelopmentData();
#pragma warning restore CS4014
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();