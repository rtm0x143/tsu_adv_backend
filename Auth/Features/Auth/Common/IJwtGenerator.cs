using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.Features.Auth.Common;

public interface IJwtGenerator
{
    JwtSecurityToken BuildToken(IEnumerable<Claim> claims);
    string WriteToken(JwtSecurityToken token);
}

/// <summary>
/// Extensions for <see cref="IJwtGenerator"/>
/// </summary>
public static class JwtGeneratorExtensions
{
    /// <summary>
    /// <see cref="IJwtGenerator.BuildToken"/> and then <see cref="IJwtGenerator.WriteToken"/>
    /// </summary>
    /// <returns>Token string</returns>
    public static string IssueToken(this IJwtGenerator jwtGenerator, IEnumerable<Claim> claims) =>
        jwtGenerator.WriteToken(jwtGenerator.BuildToken(claims));
}