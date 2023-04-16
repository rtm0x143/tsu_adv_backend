using Auth.Features.Auth.Common;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.RequestHandlers;

namespace Auth.Features.Auth.Commands;

public record ChangePasswordDto(string CurrentPassword, string NewPassword);

/// <summary>
/// Checks current user's password, changes it to new, revokes all refresh tokens and issues new token pair
/// </summary>
/// <exception cref="KeyNotFoundException">When user with <paramref name="UserId"/> couldn't be found</exception>
/// <exception cref="ActionFailedException"></exception>
/// <exception cref="ArgumentException"></exception>
public sealed record ChangePasswordCommand(Guid UserId, string CurrentPassword, string NewPassword) 
    : IRequestWithException<TokensResult>;

[RequestHandlerInterface]
public interface IChangePassword : IRequestHandlerWithException<ChangePasswordCommand, TokensResult>
{
}