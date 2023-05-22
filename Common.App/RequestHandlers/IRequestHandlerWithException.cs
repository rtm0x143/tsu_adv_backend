using Common.Domain.ValueTypes;
using OneOf;

namespace Common.App.RequestHandlers;

/// <typeparam name="TResult">Type of successful execution result</typeparam>
/// <typeparam name="TException">Type of exception</typeparam>
/// <returns><see cref="TResult"/> if executed successfully or <see cref="TException"/> see <c>exceptions</c> for details</returns>
public interface IRequestWithException<TResult, TException> : IRequest<OneOf<TResult, TException>>
    where TException : Exception
{
}

/// <inheritdoc cref="IRequestWithException{TResult,TException}"/>
public interface IRequestWithException<TResult> : IRequestWithException<TResult, Exception>
{
}

/// <inheritdoc cref="IRequestWithException{TResult,TException}"/>
public interface IRequestWithException : IRequestWithException<EmptyResult, Exception>
{
}

public interface IRequestHandlerWithException<in TRequest, TResult, TException>
    : IRequestHandler<TRequest, OneOf<TResult, TException>>
    where TException : Exception
    where TRequest : IRequest<OneOf<TResult, TException>>
{
}

public interface IRequestHandlerWithException<in TRequest, TResult>
    : IRequestHandlerWithException<TRequest, TResult, Exception>
    where TRequest : IRequest<OneOf<TResult, Exception>>
{
}

public interface IRequestHandlerWithException<in TRequest>
    : IRequestHandlerWithException<TRequest, EmptyResult, Exception>
    where TRequest : IRequest<OneOf<EmptyResult, Exception>>
{
}