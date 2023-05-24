using AdminPanel.Services;
using OneOf;

namespace AdminPanel.Infra.Http;

public partial class AuthHttpClient : IAuthService
{
    private record LoginDto(string Email, string Password);

    public Task<OneOf<LoginResult, Exception>> Login(string email, string password)
    {
        return SendAsJsonCheckedAsync<LoginResult, LoginDto>(
            method: HttpMethod.Post,
            requestUri: "auth/login",
            payload: new LoginDto(email, password));
    }

    private record RefreshDto(string RefreshToken);

    public Task<OneOf<LoginResult, Exception>> Refresh(string refreshToken)
    {
        return SendAsJsonCheckedAsync<LoginResult, RefreshDto>(
            method: HttpMethod.Post,
            requestUri: "auth/refresh",
            payload: new RefreshDto(refreshToken));
    }
}