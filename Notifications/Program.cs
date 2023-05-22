using System.Reflection;
using Common.App.Configure;
using Common.App.Configure.Swagger;
using Common.App.RequestHandlers;
using Common.App.Utils;
using Common.Infra.Auth.Configure;
using Common.Infra.Messaging;
using Common.Infra.Services.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Notifications.Hubs;
using Notifications.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR().AddJsonProtocol();

builder.Services.AddProblemDetails();
builder.Services.AddCommonAppServices();
builder.Services.AddRouting(configure => configure.LowercaseUrls = true);

builder.Services.AddCommonVersioning();
builder.Services.AddCommonSwagger();

builder.Services.AddDbContext<NotificationsDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetRequiredString("NOTIFICATIONS_DB_CONN", "ConnectionStrings:Default")));

builder.Services.AddCommonJwtServices(builder.Configuration)
    .AddCommonJwtBearerAuth(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/api/hubs"))
                {
                    context.Token = accessToken;
                    return Task.CompletedTask;
                }

                return Task.CompletedTask;
            }
        };
    });

// TODO : refactor
// builder.Services.Configure<JwtBearerOptions>();
builder.Services.AddCommonAuthorization();
builder.Services.AddRequestHandlersFrom(Assembly.GetExecutingAssembly());
builder.Host.ConfigureMessageBus<NotificationsMessageBusConfiguration>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseCommonSwagger();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationsHub>("/api/hubs/notifications");

app.Run();