using Auth.Features.Auth.Common;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.RequestHandlers;
using Common.Domain.Exceptions;
using OneOf;

namespace Auth.Features.Auth.Commands;

public record RefreshCommand(string RefreshToken) : 
    IRequest<OneOf<TokensResult, ActionFailedException, ArgumentException, KeyNotFoundException>>;

[RequestHandlerInterface]
public interface IRefresh : IRequestHandler<
    RefreshCommand,
    OneOf<TokensResult, ActionFailedException, ArgumentException, KeyNotFoundException>>
{
}