using Common.App.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Common.Infra.Auth.Configure;

internal class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly ITokenValidationParametersProvider _parametersProvider;

    public ConfigureJwtBearerOptions(ITokenValidationParametersProvider parametersProvider)
        => _parametersProvider = parametersProvider;

    public void Configure(JwtBearerOptions options)
        => options.TokenValidationParameters = _parametersProvider.ValidationParameters;

    public void Configure(string? name, JwtBearerOptions options) => Configure(options);
}