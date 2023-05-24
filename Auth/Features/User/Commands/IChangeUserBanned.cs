using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Auth.Features.User.Commands;

public sealed record ChangeUserBannedCommand(Guid UserId, bool IsBanned) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IChangeUserBanned : IRequestHandlerWithException<ChangeUserBannedCommand, EmptyResult>
{
}