using Common.App.Exceptions;
using Common.Domain.Exceptions;
using OneOf;

namespace Auth.Features.Auth.Common;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }
    public bool IsDropped { get; private set; }

    public static OneOf<RefreshToken, ArgumentException> CreateNew(Guid associatedUserId, TimeSpan lifeTime) =>
        Construct(Guid.NewGuid(), associatedUserId, DateTime.UtcNow.Add(lifeTime), false);

    public static OneOf<RefreshToken, ArgumentException> Construct(Guid id, Guid associatedUserId, DateTime expiration,
        bool isUsed)
    {
        if (id == default) return new HadDefaultValueException(nameof(id));
        if (associatedUserId == default) return new HadDefaultValueException(nameof(associatedUserId));
        if (expiration == default) return new HadDefaultValueException(nameof(expiration));

        return new RefreshToken
        {
            Id = id,
            UserId = associatedUserId,
            IsUsed = isUsed,
            ExpiresAt = expiration
        };
    }

    public async Task<OneOf<RefreshToken, ActionFailedException>> ExecuteRefresh(IRefreshTokenHandler handler)
    {
        if (IsUsed)
        {
            await handler.DropFamily(this);
            IsDropped = true;
            return new ActionFailedException("Reuse of refresh token");
        }

        if (ExpiresAt <= DateTime.UtcNow) return new ActionFailedException("Refresh token already expired");
        IsUsed = true;
        return (await handler.ReIssue(this)).Match<OneOf<RefreshToken, ActionFailedException>>(
            token => token,
            ex => new ActionFailedException("While reissuing token", ex));
    }
}