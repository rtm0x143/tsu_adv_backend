using OneOf;

namespace AdminPanel.Services;

public record LoginResult(string AccessToken, string RefreshToken);

public interface IAuthService
{
    Task<OneOf<LoginResult, Exception>> Login(string email, string password);
    Task<OneOf<LoginResult, Exception>> Refresh(string refreshToken);
}