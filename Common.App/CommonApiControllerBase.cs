using System.Security.Claims;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.RequestHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace Common.App;

[ApiController]
[VersioningApiRoute]
public abstract class CommonApiControllerBase<TConcreteController> : ControllerBase
    where TConcreteController : CommonApiControllerBase<TConcreteController>
{
    [FromServices] public IExceptionsDescriber ExceptionsDescriber { get; init; } = default!;
    [FromServices] public IAuthorizationService AuthService { get; init; } = default!;
    [FromServices] public ILogger<TConcreteController> Logger { get; init; } = default!;
    [FromServices] public IOptions<ClaimsIdentityOptions> ClaimsIdentityOptions { get; init; } = default!;

    [NonAction]
    protected virtual string? GetUserId() => User.FindFirstValue(ClaimsIdentityOptions.Value.UserIdClaimType);

    [NonAction]
    public virtual ActionResult InvalidTokenPayload(string? details = null) => Problem(
        title: "Invalid payload in token",
        statusCode: StatusCodes.Status400BadRequest,
        detail: details);

    /// <summary>
    /// Executes <see cref="IRequest{TResult}"/> and wraps response into <see cref="ActionResult{TValue}"/> 
    /// </summary>
    /// <param name="handler">An request handler to use for request execution </param>
    /// <param name="request">An request to execute </param>
    /// <returns>Request response wrapped into <see cref="ActionResult{TValue}"/></returns>
    protected virtual Task<ActionResult<TResponse>> ExecuteRequest<TRequest, TResponse>(
        IRequestHandler<TRequest, TResponse> handler,
        TRequest request)
        where TRequest : IRequest<TResponse>
        => handler.Execute(request).ContinueWith<ActionResult<TResponse>>(t => Ok(t.Result));

    /// <summary>
    /// Executes <see cref="IRequestWithException{TResult,TException}"/> and wraps response
    /// into <see cref="ActionResult{TValue}"/> describing exception using <see cref="ExceptionsDescriber"/>
    /// </summary>
    /// <param name="handler">An request handler to use for request execution </param>
    /// <param name="request">An request to execute </param>
    /// <returns>Request response wrapped into <see cref="ActionResult{TValue}"/></returns>
    protected virtual Task<ActionResult<TResponse>> ExecuteRequest<TRequest, TResponse, TException>(
        IRequestHandlerWithException<TRequest, TResponse, TException> handler,
        TRequest request)
        where TRequest : IRequestWithException<TResponse, TException>
        where TException : Exception
    {
        return handler.Execute(request).ContinueWith<ActionResult<TResponse>>(
            t => t.Result.IsT0
                ? Ok(t.Result.AsT0)
                : ExceptionsDescriber.Describe(t.Result.AsT1));
    }

    /// <summary>
    /// Executes <see cref="IRequestWithException{TResult,TException}"/> where <c>TResult</c> is <see cref="EmptyResult"/>
    /// and wraps response into <see cref="ActionResult{TValue}"/> describing exception using <see cref="ExceptionsDescriber"/>
    /// </summary>
    /// <param name="handler">An request handler to use for request execution </param>
    /// <param name="request">An request to execute </param>
    /// <returns>Request response wrapped into <see cref="ActionResult"/></returns>
    protected virtual Task<ActionResult> ExecuteRequest<TRequest, TException>(
        IRequestHandlerWithException<TRequest, EmptyResult, TException> handler,
        TRequest request)
        where TRequest : IRequestWithException<EmptyResult, TException>
        where TException : Exception
    {
        return handler.Execute(request).ContinueWith<ActionResult>(
            t => t.Result.IsT0 ? Ok() : ExceptionsDescriber.Describe(t.Result.AsT1));
    }
}