using Auth.Features.Auth.Common;
using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.Exceptions;

namespace Auth.Features.Auth.Commands;

/// <exception cref="ActionFailedException">When refresh failed</exception>
/// <exception cref="KeyNotFoundException"></exception>
/// <exception cref="ArgumentException"></exception>
public sealed record RefreshCommand(string RefreshToken) : IRequestWithException<TokensResult, Exception>;

[RequestHandlerInterface]
public interface IRefresh : IRequestHandlerWithException<RefreshCommand, TokensResult>
{
}