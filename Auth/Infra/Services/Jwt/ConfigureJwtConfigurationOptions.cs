using Common.Infra.Services.Jwt;
using Microsoft.Extensions.Options;

namespace Auth.Infra.Services.Jwt;

internal class ConfigureJwtConfigurationOptions : IConfigureOptions<JwtConfigurationOptions>
{
    private readonly IConfigureOptions<JwtValidationOptions> _configure;
    public ConfigureJwtConfigurationOptions(IConfigureOptions<JwtValidationOptions> configure) => _configure = configure;

    public void Configure(JwtConfigurationOptions options) => _configure.Configure(options);
}