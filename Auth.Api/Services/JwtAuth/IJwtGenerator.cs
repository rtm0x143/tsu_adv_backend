using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.Api.Services.JwtAuth;

public interface IJwtGenerator
{
    JwtSecurityToken BuildToken(IEnumerable<Claim> claims);
    string WriteToken(JwtSecurityToken token);
}
