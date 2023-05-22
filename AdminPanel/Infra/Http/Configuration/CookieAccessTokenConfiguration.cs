using System.Net.Http.Headers;
using Common.Infra.HttpClients;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace AdminPanel.Infra.Http.Configuration;

public class CookieAccessTokenConfiguration : NamedHttpClientConfigurationBase
{
    protected override void ConfigureClient(IServiceProvider provider, HttpClient client)
    {
        var context = provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        var cookieParametersOptions = provider.GetRequiredService<IOptions<CookieParametersOptions>>().Value;

        if (context?.Request.Cookies[cookieParametersOptions.AccessTokenParameterName] is string accessToken)
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);
    }

    public CookieAccessTokenConfiguration(string name) : base(name)
    {
    }
}