using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.Infra.Services;

public interface IJwtGenerator
{
    JwtSecurityToken BuildToken(IEnumerable<Claim> claims);
    string WriteToken(JwtSecurityToken token);
}
