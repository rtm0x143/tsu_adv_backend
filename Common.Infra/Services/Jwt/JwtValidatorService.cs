using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.App.Services.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Common.Infra.Services.Jwt;

internal class JwtValidatorService : IJwtValidator, ITokenValidationParametersProvider
{
    /// <summary>
    /// Creates new <see cref="TokenValidationParameters"/> using data from <paramref name="props"/>
    /// </summary>
    /// <inheritdoc cref="JwtConfigurationProperties.ReadConfiguration(IConfiguration)"/>
    public static TokenValidationParameters CreateValidationParameters(JwtConfigurationProperties props)
    {
        var parameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(props.SigningKey)),
            ValidAlgorithms = props.ValidAlgorithms,
            ValidateIssuer = !props.Issuers.IsNullOrEmpty(),
            ValidateAudience = !props.Audiences.IsNullOrEmpty()
        };

        parameters.ValidIssuers = parameters.ValidateIssuer ? props.Issuers.Append(props.ApplicationId) : null;
        parameters.ValidAudiences = parameters.ValidateAudience ? props.Audiences.Append(props.ApplicationId) : null;

        return parameters;
    }

    public TokenValidationParameters ValidationParameters { get; }

    /// <summary>
    /// Creates new <see cref="JwtValidatorService"/> instance
    /// </summary>
    public JwtValidatorService(IOptions<JwtConfigurationProperties> options)
    {
        ValidationParameters = CreateValidationParameters(options.Value);
    }

    public bool ValidateToken(string token, [NotNullWhen(true)] out ClaimsPrincipal? claims, [NotNullWhen(false)] out Exception? problem)
    {
        try
        {
            claims = new JwtSecurityTokenHandler().ValidateToken(token, ValidationParameters, out var validToken);
            problem = null;
        }
        catch (Exception e)
        {
            problem = e;
            claims = null;
        }

        return claims != null;
    }
}
