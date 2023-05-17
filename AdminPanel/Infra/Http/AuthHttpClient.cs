using AdminPanel.Services;
using Common.Infra.HttpClients;

namespace AdminPanel.Infra.Http;

public class AuthHttpClient : IAuthService
{
    public const string Name = "Auth";

    public static INamedHttpClientConfiguration Configuration { get; } = new CommonNamedHttpClientConfiguration(Name);

    private readonly HttpClient _httpClient;

    public AuthHttpClient(IHttpClientFactory httpClientFactory) => _httpClient = httpClientFactory.CreateClient(Name);

    private record LoginDto(string Email, string Password);

    public async Task<LoginResult> Login(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("login", new LoginDto(email, password));
        return await response.Content.ReadFromJsonAsync<LoginResult>()
               ?? throw new HttpRequestException("Invalid response content returned");
    }
}