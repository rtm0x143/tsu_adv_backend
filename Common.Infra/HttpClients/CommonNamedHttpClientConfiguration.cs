using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Infra.HttpClients;

public class CommonNamedHttpClientConfiguration : NamedHttpClientConfigurationBase
{
    public string ApiKeyHeaderName { get; init; } = "X-API-KEY";

    protected override void ConfigureClient(IServiceProvider provider, HttpClient client)
    {
        var optionsMonitor = provider.GetRequiredService<IOptionsMonitor<ExternalApiOptions>>();
        var options = optionsMonitor.Get(Name);
        client.BaseAddress = options.Url;
        if (options.ApiBearer != null)
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, options.ApiBearer);
        if (options.ApiKey != null) client.DefaultRequestHeaders.Add(ApiKeyHeaderName, options.ApiKey);
    }

    public CommonNamedHttpClientConfiguration(string name) : base(name)
    {
    }
}