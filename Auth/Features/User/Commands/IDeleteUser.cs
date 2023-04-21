using Common.App.Attributes;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Auth.Features.User.Commands;

public sealed record DeleteUserCommand(Guid Id) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IDeleteUser : IRequestHandlerWithException<DeleteUserCommand, EmptyResult>
{
}