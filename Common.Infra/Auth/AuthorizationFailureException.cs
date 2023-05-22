using Microsoft.AspNetCore.Authorization;

namespace Common.Infra.Auth;

public class AuthorizationFailureException : AuthorizationFailureReason
{
    public Exception Exception { get; }

    public AuthorizationFailureException(IAuthorizationHandler handler, Exception exception)
        : base(handler, exception.Message)
        => Exception = exception;
}

public static class AuthorizationHandlerExtensions
{
    public static AuthorizationFailureException FailureFromException(this IAuthorizationHandler handler,
        Exception exception)
        => new(handler, exception);
}
