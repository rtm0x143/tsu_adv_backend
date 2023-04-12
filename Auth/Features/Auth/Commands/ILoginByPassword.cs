using System.ComponentModel.DataAnnotations;
using Auth.Features.Auth.Common;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.UseCases;
using OneOf;

namespace Auth.Features.Auth.Commands;

public record LoginByPasswordCommand : 
    IAsyncRequest<OneOf<TokensResult, KeyNotFoundException, ActionFailedException>>
{
    [EmailAddress] public required string Email { get; set; }
    public required string Password { get; set; }
}

[UseCaseInterface]
public interface ILoginByPassword : IAsyncUseCase<LoginByPasswordCommand,
    OneOf<TokensResult, KeyNotFoundException, ActionFailedException>>
{
}
