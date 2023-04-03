using Common.Api.jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Common.Api.Configure.Jwt;

public class ConfigureJwtBearerOptions : IConfigureOptions<JwtBearerOptions>
{
    private readonly IJwtValidator _validator;

    public ConfigureJwtBearerOptions(IJwtValidator validator)
    {
        _validator = validator;
    }

    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = _validator.ValidationParameters;
    }
}
