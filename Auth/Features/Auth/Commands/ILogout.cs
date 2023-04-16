using Common.App.Attributes;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Auth.Features.Auth.Commands;

public sealed record LogoutCommand(Guid UserId, string RefreshToken) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface ILogout : IRequestHandlerWithException<LogoutCommand, EmptyResult>
{
}