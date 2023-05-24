using Auth.Infra.Data.Entities;
using OneOf;

namespace Auth.Features.Auth.Common;

public interface IRefreshTokenHandler
{
    Task<OneOf<RefreshToken, ArgumentException>> IssuerFor(AppUser user);
    Task<OneOf<RefreshToken, ArgumentException>> ReIssue(RefreshToken token);
    ValueTask Revoke(RefreshToken token);
    Task DropFamily(RefreshToken token);
    Task DropFamily(AppUser id);
    string Write(RefreshToken token);

    /// <exception cref="ArgumentException">When token invalid</exception>
    /// <exception cref="KeyNotFoundException">When token unknown</exception>
    /// <returns><see cref="RefreshToken"/> or exception</returns>
    ValueTask<OneOf<RefreshToken, Exception>> Read(string refreshTokenString);
}