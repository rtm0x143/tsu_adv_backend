using Auth.Features.Auth.Common;
using Auth.Infra.Data.Entities;
using OneOf;

namespace Auth.Infra.Services.Refresh;

public static class UserRefreshTokenExtensions
{
    public static OneOf<RefreshToken, ArgumentException> ToRefreshTokenModel(this UserRefreshToken token,
        IRefreshTokenHandler handler) =>
        RefreshToken.Construct(handler, token.Id, token.UserId, token.ExpiresAt, token.IsUsed);
}