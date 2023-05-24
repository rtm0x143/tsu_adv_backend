using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Notifications.Infra.Configuration;

public class AccessTokenFromQueryConfiguration : IPostConfigureOptions<JwtBearerOptions>
{
    public void PostConfigure(string? name, JwtBearerOptions options)
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
    }
}