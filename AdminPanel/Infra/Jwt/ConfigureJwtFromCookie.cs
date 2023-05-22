using AdminPanel.Infra.Http.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace AdminPanel.Infra.Jwt;

public class ConfigureJwtFromCookie : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly IOptionsMonitor<CookieParametersOptions> _cookieParametersOptions;

    public ConfigureJwtFromCookie(IOptionsMonitor<CookieParametersOptions> options)
    {
        _cookieParametersOptions = options;
    }

    private Task _onMessageReceived(MessageReceivedContext context)
    {
        if (context.Token != null) return Task.CompletedTask;
        context.Token = context.Request.Cookies[_cookieParametersOptions.CurrentValue.AccessTokenParameterName];
        return Task.CompletedTask;
    }

    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        options.Events = new() { OnMessageReceived = _onMessageReceived };
    }
}