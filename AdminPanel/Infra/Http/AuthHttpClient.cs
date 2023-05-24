using AdminPanel.Infra.Http.Configuration;
using Common.Infra.HttpClients;

namespace AdminPanel.Infra.Http;

public partial class AuthHttpClient : CheckedHttpClient
{
    public const string Name = "Auth";

    public static INamedHttpClientConfiguration Configuration { get; } = new CookieAccessTokenConfiguration(Name);

    public AuthHttpClient(IHttpClientFactory factory) : base(factory.CreateClient(Name))
    {
    }
}