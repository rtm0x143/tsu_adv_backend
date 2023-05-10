using System.ComponentModel.DataAnnotations;
using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Auth.Features.Auth.Commands;

public sealed record RequestPasswordRecoverCommand
    : IRequestWithException<EmptyResult, KeyNotFoundException>
{
    [EmailAddress] public required string UsersEmail { get; init; }  // explicit prop for framework use
}

[RequestHandlerInterface]
public interface IRequestPasswordRecover
    : IRequestHandlerWithException<RequestPasswordRecoverCommand, EmptyResult, KeyNotFoundException>
{
}