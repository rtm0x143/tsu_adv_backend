namespace Common.App.RequestHandlers;

/// <summary>
/// Marks class as Command/Query without result type
/// </summary>
public interface IRequest
{
}

/// <summary>
/// Describes interface of handler which can process requests of type <typeparamref name="TRequest"/> 
/// </summary>
public interface IRequestHandler<in TRequest> where TRequest : IRequest
{
    Task Execute(TRequest request);
}


/// <summary>
/// Marks class as Command/Query with expected execution result of type <see cref="TRequestResult"/>
/// </summary>
/// <typeparam name="TRequestResult">Expected result type</typeparam>
public interface IRequest<TRequestResult>
{
}

/// <summary>
/// Generic async handler interface
/// </summary>
/// <typeparam name="TRequest">Command/Query which should be associated with only one handler</typeparam>
/// <typeparam name="TResult">Output type</typeparam>
public interface IRequestHandler<in TRequest, TResult> where TRequest : IRequest<TResult>
{
    Task<TResult> Execute(TRequest request);
}
