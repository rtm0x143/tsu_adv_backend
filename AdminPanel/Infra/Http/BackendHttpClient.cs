using AdminPanel.Infra.Http.Configuration;
using Common.Infra.HttpClients;

namespace AdminPanel.Infra.Http;

public partial class BackendHttpClient : CheckedHttpClient
{
    public const string Name = "Backend";
    
    public static INamedHttpClientConfiguration Configuration { get; } = new CookieAccessTokenConfiguration(Name);

    public BackendHttpClient(IHttpClientFactory factory) : base(factory.CreateClient(Name))
    {
    }
}