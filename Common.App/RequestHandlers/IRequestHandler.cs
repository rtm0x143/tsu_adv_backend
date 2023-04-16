namespace Common.App.RequestHandlers;

/// <summary>
/// Marks class as Command/Query with expected execution result of type <see cref="TRequestResult"/>
/// </summary>
/// <typeparam name="TRequestResult">Expected result type</typeparam>
public interface IRequest<TRequestResult>
{
}

/// <summary>
/// Generic async use-case interface
/// </summary>
/// <typeparam name="TRequest">Command/Query which should be associated with only one use-case</typeparam>
/// <typeparam name="TResult">Output type</typeparam>
public interface IRequestHandler<in TRequest, TResult> where TRequest : IRequest<TResult>
{
    Task<TResult> Execute(TRequest request);
}
