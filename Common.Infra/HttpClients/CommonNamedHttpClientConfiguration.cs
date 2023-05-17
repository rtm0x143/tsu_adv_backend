using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Infra.HttpClients;

public class CommonNamedHttpClientConfiguration : INamedHttpClientConfiguration
{
    public string Name { get; }
    public string ApiKeyHeaderName { get; init; } = "X-API-KEY";

    public Action<IServiceProvider, HttpClient> ConfigureClientMethod => ConfigureClient;

    protected virtual void ConfigureClient(IServiceProvider provider, HttpClient client)
    {
        var optionsFactory = provider.GetRequiredService<IOptionsFactory<ExternalApiOptions>>();
        var options = optionsFactory.Create(Name);
        client.BaseAddress = options.Url;
        if (options.ApiBearer != null)
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, options.ApiBearer);
        if (options.ApiKey != null) client.DefaultRequestHeaders.Add(ApiKeyHeaderName, options.ApiKey);
    }

    public CommonNamedHttpClientConfiguration(string name) => Name = name;
}