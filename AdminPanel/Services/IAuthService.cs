namespace AdminPanel.Services;

public record LoginResult(string AccessToken, string RefreshToken);

public interface IAuthService
{
    Task<LoginResult> Login(string email, string password);
}