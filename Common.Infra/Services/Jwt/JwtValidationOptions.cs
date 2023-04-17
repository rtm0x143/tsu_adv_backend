namespace Common.Infra.Services.Jwt;

public class JwtValidationOptions
{
    public const string ConfigurationSectionName = "Jwt";

    public string SigningKey { get; set; } = default!;
    public string[] Issuers { get; set; } = Array.Empty<string>();
    public string[] Audiences { get; set; } = Array.Empty<string>();
    public string[] ValidAlgorithms { get; set; } = Array.Empty<string>();
    public string ApplicationId { get; set; } = default!;
}