using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Auth.Features.User.Commands;

/// <summary>
/// Just sets password to new one
/// </summary>
/// <exception cref="KeyNotFoundException">When user with <paramref name="UserId"/> couldn't be found</exception>
/// <exception cref="UnsuitableDataException"></exception>
public sealed record SetPasswordCommand(Guid UserId, string Password) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface ISetPassword : IRequestHandlerWithException<SetPasswordCommand, EmptyResult>
{
}