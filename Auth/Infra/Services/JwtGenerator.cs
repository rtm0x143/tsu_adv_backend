using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Auth.Features.Common;
using Common.App.Services.Jwt;
using Common.Infra.Services.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infra.Services;

public class JwtGenerator : IJwtGenerator
{
    private readonly JwtSecurityTokenHandler _handler = new();
    private readonly JwtConfigurationProperties _jwtProps;
    private readonly SigningCredentials _signingCredentials;

    public JwtGenerator(IOptions<JwtConfigurationProperties> options, ITokenValidationParametersProvider provider)
    {
        _jwtProps = options.Value;
        _signingCredentials = new SigningCredentials(provider.ValidationParameters.IssuerSigningKey,
            _jwtProps.SigningAlgorithm);
    }

    public JwtSecurityToken BuildToken(IEnumerable<Claim> claims)
    {
        var identity = new ClaimsIdentity(claims, null, /*JwtRegisteredClaimNames.Name*/ null, null);
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        
        return _handler.CreateJwtSecurityToken(
            subject: identity,
            issuer: _jwtProps.ApplicationId,
            audience: _jwtProps.ApplicationId,
            expires: DateTime.UtcNow.Add(_jwtProps.LifeTime),
            signingCredentials: _signingCredentials);
    }

    public string WriteToken(JwtSecurityToken token) => _handler.WriteToken(token);
}