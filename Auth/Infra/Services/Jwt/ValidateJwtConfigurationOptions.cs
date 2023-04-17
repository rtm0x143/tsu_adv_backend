using Common.Infra.Services.Jwt;
using Microsoft.Extensions.Options;

namespace Auth.Infra.Services.Jwt;

internal class ValidateJwtConfigurationOptions : IValidateOptions<JwtConfigurationOptions>
{
    private readonly IValidateOptions<JwtValidationOptions> _validate;
    public ValidateJwtConfigurationOptions(IValidateOptions<JwtValidationOptions> validate) => _validate = validate;

    public ValidateOptionsResult Validate(string? name, JwtConfigurationOptions options) =>
        _validate.Validate(name, options);
}