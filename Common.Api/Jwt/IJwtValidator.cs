using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Common.Api.jwt;

public interface IJwtValidator
{
    TokenValidationParameters ValidationParameters { get; }
    bool ValidateToken(string token, [NotNullWhen(true)] out ClaimsPrincipal? claims, [NotNullWhen(false)] out Exception? problem);
}
