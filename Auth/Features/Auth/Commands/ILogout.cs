using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Auth.Features.Auth.Commands;

public sealed record LogoutCommand(Guid UserId, string RefreshToken) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface ILogout : IRequestHandlerWithException<LogoutCommand, EmptyResult>
{
}