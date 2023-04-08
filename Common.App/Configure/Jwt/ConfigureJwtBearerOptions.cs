using Common.App.Services.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Common.App.Configure.Jwt;

internal class ConfigureJwtBearerOptions : IConfigureOptions<JwtBearerOptions>
{
    private readonly ITokenValidationParametersProvider _parametersProvider;

    public ConfigureJwtBearerOptions(ITokenValidationParametersProvider parametersProvider)
    {
        _parametersProvider = parametersProvider;
    }

    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = _parametersProvider.ValidationParameters;
    }
}
