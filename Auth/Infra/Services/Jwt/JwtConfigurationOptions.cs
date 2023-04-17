using Common.Infra.Services.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infra.Services.Jwt;

public class JwtConfigurationOptions : JwtValidationOptions
{
    public TimeSpan LifeTime { get; set; }
    public string SigningAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
}