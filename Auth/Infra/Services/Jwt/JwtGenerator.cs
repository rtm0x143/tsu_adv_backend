using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Auth.Features.Auth.Common;
using Common.App.Services.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infra.Services.Jwt;

public class JwtGenerator : IJwtGenerator
{
    private readonly JwtSecurityTokenHandler _handler = new();
    private readonly JwtConfigurationOptions _jwtOptions;
    private readonly SigningCredentials _signingCredentials;

    public JwtGenerator(IOptionsMonitor<JwtConfigurationOptions> optionsMonitor,
        ITokenValidationParametersProvider provider)
    {
        _jwtOptions = optionsMonitor.CurrentValue;
        _signingCredentials = new SigningCredentials(provider.ValidationParameters.IssuerSigningKey,
            _jwtOptions.SigningAlgorithm);
    }

    public JwtSecurityToken BuildToken(IEnumerable<Claim> claims)
    {
        var identity = new ClaimsIdentity(claims, null, null, null);
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        return _handler.CreateJwtSecurityToken(
            subject: identity,
            issuer: _jwtOptions.ApplicationId,
            audience: _jwtOptions.ApplicationId,
            expires: DateTime.UtcNow.Add(_jwtOptions.LifeTime),
            signingCredentials: _signingCredentials);
    }

    public string WriteToken(JwtSecurityToken token) => _handler.WriteToken(token);
}