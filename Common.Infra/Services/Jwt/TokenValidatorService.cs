using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.App.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Common.Infra.Services.Jwt;

internal class TokenValidatorService : ITokenValidator, ITokenValidationParametersProvider
{
    /// <summary>
    /// Creates new <see cref="TokenValidationParameters"/> using data from <paramref name="options"/>
    /// </summary>
    /// <inheritdoc cref="JwtConfigurationProperties.ReadConfiguration(IConfiguration)"/>
    public static TokenValidationParameters CreateValidationParameters(JwtValidationOptions options)
    {
        var parameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey)),
            ValidAlgorithms = options.ValidAlgorithms,
            ValidateIssuer = !options.Issuers.IsNullOrEmpty(),
            ValidateAudience = !options.Audiences.IsNullOrEmpty()
        };

        parameters.ValidIssuers = parameters.ValidateIssuer ? options.Issuers.Append(options.ApplicationId) : null;
        parameters.ValidAudiences = parameters.ValidateAudience ? options.Audiences.Append(options.ApplicationId) : null;

        return parameters;
    }

    public TokenValidationParameters ValidationParameters { get; }

    /// <summary>
    /// Creates new <see cref="TokenValidatorService"/> instance
    /// </summary>
    public TokenValidatorService(IOptions<JwtValidationOptions> options)
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
