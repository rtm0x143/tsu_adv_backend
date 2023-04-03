using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Api.jwt;
using Microsoft.Extensions.Options;

namespace Auth.Api.Services.JwtAuth;

public class JwtGenerator : IJwtGenerator
{
    private readonly JwtConfigurationProperties _jwtProps;

    public JwtGenerator(IOptions<JwtConfigurationProperties> options)
    {
        _jwtProps = options.Value;
    }

    public JwtSecurityToken BuildToken(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtProps.SigningKey));

        return new JwtSecurityToken(
            issuer: _jwtProps.ApplicationId,
            audience: _jwtProps.ApplicationId,
            claims: claims.Append(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())),
            expires: DateTime.UtcNow.Add(_jwtProps.LifeTime),
            signingCredentials: new SigningCredentials(securityKey, _jwtProps.SigningAlgorithm));
    }

    public string WriteToken(JwtSecurityToken token) => new JwtSecurityTokenHandler().WriteToken(token);
}
