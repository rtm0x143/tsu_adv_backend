namespace Auth.Infra.Services.Refresh;

public class RefreshTokenConfigurationProperties
{
    public const string ConfigurationSection = "RefreshToken";
    
    public TimeSpan LifeTime { get; set; }
}