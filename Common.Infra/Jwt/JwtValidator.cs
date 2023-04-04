using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Api.jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Common.Infra.Jwt;

internal class JwtValidator : IJwtValidator, ITokenValidationParametersProvider
{
    /// <summary>
    /// Creates new <see cref="TokenValidationParameters"/> using data from <paramref name="props"/>
    /// </summary>
    /// <inheritdoc cref="JwtConfigurationProperties.ReadConfiguration(IConfiguration)"/>
    public static TokenValidationParameters CreateValidationParameters(JwtConfigurationProperties props)
        => new()
        {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(props.SigningKey)),
            ValidAlgorithms = props.ValidAlgorithms,
            ValidateIssuer = true,
            ValidIssuers = props.Issuers.Append(props.ApplicationId),
            ValidateAudience = true,
            ValidAudiences = props.Audiences.Append(props.ApplicationId),
        };

    public TokenValidationParameters ValidationParameters { get; }

    /// <summary>
    /// Creates new <see cref="JwtValidator"/> instance
    /// </summary>
    public JwtValidator(IOptions<JwtConfigurationProperties> options)
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
