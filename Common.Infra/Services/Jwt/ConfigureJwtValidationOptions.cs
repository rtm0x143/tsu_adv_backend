using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Common.Infra.Services.Jwt;

internal class ConfigureJwtValidationOptions : IConfigureOptions<JwtValidationOptions>
{
    internal const string SigningKeyEnvName = "JWT_SIGNING_KEY";

    protected readonly IConfiguration Configuration;
    public ConfigureJwtValidationOptions(IConfiguration configuration) => Configuration = configuration;

    public void Configure(JwtValidationOptions options)
    {
        Configuration.GetSection(JwtValidationOptions.ConfigurationSectionName).Bind(options);
        options.SigningKey = Configuration["JWT_SIGNING_KEY"] ?? options.SigningKey;
        if (Configuration[nameof(JwtValidationOptions.ApplicationId)] is string id) options.ApplicationId = id;
    }
}