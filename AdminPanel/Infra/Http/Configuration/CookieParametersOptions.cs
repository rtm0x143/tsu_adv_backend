namespace AdminPanel.Infra.Http.Configuration;

public class CookieParametersOptions
{
    public string AccessTokenParameterName { get; set; } = default!;
    public string RefreshTokenParameterName { get; set; } = default!;
}