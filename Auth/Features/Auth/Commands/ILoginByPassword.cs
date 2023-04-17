using System.ComponentModel.DataAnnotations;
using Auth.Features.Auth.Common;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.RequestHandlers;
using OneOf;

namespace Auth.Features.Auth.Commands;

public sealed record LoginByPasswordCommand : 
    IRequest<OneOf<TokensResult, KeyNotFoundException, ActionFailedException>>
{
    [EmailAddress] public required string Email { get; init; }  // explicit prop for framework use
    public required string Password { get; set; }
}

[RequestHandlerInterface]
public interface ILoginByPassword : IRequestHandler<LoginByPasswordCommand,
    OneOf<TokensResult, KeyNotFoundException, ActionFailedException>>
{
}
