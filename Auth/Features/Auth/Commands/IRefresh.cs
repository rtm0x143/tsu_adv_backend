using Auth.Features.Auth.Common;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.UseCases;
using OneOf;

namespace Auth.Features.Auth.Commands;

public record RefreshCommand(string RefreshToken) : 
    IAsyncRequest<OneOf<TokensResult, ActionFailedException, ArgumentException, KeyNotFoundException>>;

[UseCaseInterface]
public interface IRefresh : IAsyncUseCase<
    RefreshCommand,
    OneOf<TokensResult, ActionFailedException, ArgumentException, KeyNotFoundException>>
{
}