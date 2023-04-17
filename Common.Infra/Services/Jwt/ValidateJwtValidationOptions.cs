using Microsoft.Extensions.Options;

namespace Common.Infra.Services.Jwt;

internal class ValidateJwtValidationOptions : IValidateOptions<JwtValidationOptions>
{
    private const string SigningKeyNotfound =
        $"{nameof(JwtValidationOptions.SigningKey)} not fond, try specify value " +
        $"in {JwtValidationOptions.ConfigurationSectionName} config section " +
        $"or in {ConfigureJwtValidationOptions.SigningKeyEnvName} ENV";

    private const string ApplicationIdNotFound =
        $"{nameof(JwtValidationOptions.ApplicationId)} was Null, try specify value in config";

    // ReSharper disable  ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
    public ValidateOptionsResult Validate(string? name, JwtValidationOptions options)
    {
        if (options.SigningKey == null)
            return ValidateOptionsResult.Fail(SigningKeyNotfound);
        if (options.ApplicationId == null)
            return ValidateOptionsResult.Fail(ApplicationIdNotFound);
        return ValidateOptionsResult.Success;
    }
}