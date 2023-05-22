using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Common.Infra.Services.Jwt;

public class ConfigureJwtValidationOptions : IConfigureOptions<JwtValidationOptions>
{
    internal const string SigningKeyEnvName = "JWT_SIGNING_KEY";

    protected readonly IConfiguration Configuration;
    public ConfigureJwtValidationOptions(IConfiguration configuration) => Configuration = configuration;

    public void Configure(JwtValidationOptions options)
    {
        options.SigningKey = Configuration[SigningKeyEnvName] ?? options.SigningKey;
        if (Configuration[nameof(JwtValidationOptions.ApplicationId)] is string id) options.ApplicationId = id;
    }
}