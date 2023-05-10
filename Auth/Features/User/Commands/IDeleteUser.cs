using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Auth.Features.User.Commands;

public sealed record DeleteUserCommand(Guid Id) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IDeleteUser : IRequestHandlerWithException<DeleteUserCommand, EmptyResult>
{
}