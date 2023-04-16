using OneOf;

namespace Common.App.RequestHandlers;

public interface IRequestWithException<TResult, TException> : IRequest<OneOf<TResult, TException>>
    where TException : Exception
{
}

public interface IRequestWithException<TResult> : IRequestWithException<TResult, Exception>
{
}

public interface IRequestHandlerWithException<in TRequest, TResult, TException> 
    : IRequestHandler<TRequest, OneOf<TResult, TException>>
    where TException : Exception 
    where TRequest : IRequest<OneOf<TResult, TException>>
{
}

public interface IRequestHandlerWithException<in TRequest, TResult> 
    : IRequestHandler<TRequest, OneOf<TResult, Exception>> 
    where TRequest : IRequest<OneOf<TResult, Exception>>
{
}