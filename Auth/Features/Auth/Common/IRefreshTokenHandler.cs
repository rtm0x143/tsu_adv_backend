using Auth.Features.Common;
using Auth.Infra.Data.Entities;
using OneOf;

namespace Auth.Features.Auth.Common;

public interface IRefreshTokenHandler
{
    Task<OneOf<RefreshToken, ArgumentException>> IssuerFor(AppUser user);
    Task<OneOf<RefreshToken, ArgumentException>> ReIssue(RefreshToken token);
    ValueTask Revoke(RefreshToken token);
    Task DropFamily(RefreshToken token);
    string Write(RefreshToken token);
    ValueTask<OneOf<RefreshToken, ArgumentException, KeyNotFoundException>> Read(string refreshTokenString);
}