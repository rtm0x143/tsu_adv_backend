using Common.App.Exceptions;
using OneOf;

namespace Auth.Features.Auth.Common;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }
    public bool IsDropped { get; private set; }

    protected IRefreshTokenHandler Handler { get; init; } = default!;


    public static OneOf<RefreshToken, ArgumentException> CreateNew(IRefreshTokenHandler handler,
        Guid associatedUserId, TimeSpan lifeTime) =>
        Construct(handler, Guid.NewGuid(), associatedUserId, DateTime.UtcNow.Add(lifeTime), false);

    public static OneOf<RefreshToken, ArgumentException> Construct(IRefreshTokenHandler handler, Guid id,
        Guid associatedUserId, DateTime expiration, bool isUsed)
    {
        if (id == default) return new ArgumentException(nameof(id));
        if (associatedUserId == default) return new ArgumentException(nameof(associatedUserId));
        if (expiration == default) return new ArgumentException(nameof(expiration));

        return new RefreshToken
        {
            Id = id,
            UserId = associatedUserId,
            IsUsed = isUsed,
            ExpiresAt = expiration,
            Handler = handler
        };
    }

    public async Task<OneOf<RefreshToken, ActionFailedException>> ExecuteRefresh()
    {
        if (IsUsed)
        {
            await Handler.DropFamily(this);
            IsDropped = true;
            return new ActionFailedException("Reuse of refresh token");
        }

        if (ExpiresAt <= DateTime.UtcNow) return new ActionFailedException("Refresh token already expired");
        IsUsed = true;
        return (await Handler.ReIssue(this)).Match<OneOf<RefreshToken, ActionFailedException>>(
            token => token,
            ex => new ActionFailedException("While reissuing token", ex));
    }

    public string Evaluate() => Handler.Write(this);
}