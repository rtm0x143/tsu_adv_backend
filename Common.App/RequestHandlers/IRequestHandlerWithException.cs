using Common.Domain.ValueTypes;
using OneOf;

namespace Common.App.RequestHandlers;

public interface IRequestWithException<TResult, TException> : IRequest<OneOf<TResult, TException>>
    where TException : Exception
{
}

public interface IRequestWithException<TResult> : IRequestWithException<TResult, Exception>
{
}

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