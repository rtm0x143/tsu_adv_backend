using Common.App.Configure;
using Common.App.Configure.Swagger;
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
    .AddCommonJwtBearerAuth();

// TODO : refactor
builder.Services.Configure<JwtBearerOptions>(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["AccessToken"];
            if (!string.IsNullOrEmpty(accessToken) &&
                context.HttpContext.Request.Path.StartsWithSegments("/api/hubs"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCommonAuthorization();

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