using AdminPanel.Infra.Http.Configuration;
using AdminPanel.Models;
using AdminPanel.Services;
using Common.Infra.HttpClients;
using OneOf;

namespace AdminPanel.Infra.Http;

public class AuthHttpClient : IAuthService, IProfileRepository
{
    public const string Name = "Auth";

    public static INamedHttpClientConfiguration Configuration { get; } = new CookieAccessTokenConfiguration(Name);

    private readonly HttpClient _httpClient;

    public AuthHttpClient(IHttpClientFactory httpClientFactory) => _httpClient = httpClientFactory.CreateClient(Name);

    private record LoginDto(string Email, string Password);

    public Task<OneOf<LoginResult, Exception>> Login(string email, string password)
    {
        return _httpClient.SendAsJsonCheckedAsync<LoginResult, LoginDto>(
            method: HttpMethod.Post,
            requestUri: "auth/login",
            payload: new LoginDto(email, password));
    }

    private record RefreshDto(string RefreshToken);

    public Task<OneOf<LoginResult, Exception>> Refresh(string refreshToken)
    {
        return _httpClient.SendAsJsonCheckedAsync<LoginResult, RefreshDto>(
            method: HttpMethod.Post,
            requestUri: "auth/refresh",
            payload: new RefreshDto(refreshToken));
    }

    public Task<OneOf<ProfileViewModel, Exception>> GetSelfProfile()
    {
        return _httpClient.SendAsJsonCheckedAsync<ProfileViewModel>(HttpMethod.Get, "user/profile");
    }
}