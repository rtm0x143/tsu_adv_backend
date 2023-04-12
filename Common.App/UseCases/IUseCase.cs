namespace Common.App.UseCases;

/// <summary>
/// Marks class as Command/Query with expected execution result of type <see cref="TRequestResult"/>
/// </summary>
/// <typeparam name="TRequestResult">Expected result type</typeparam>
public interface IRequest<TRequestResult>
{
}

/// <summary>
/// Generic use-case interface
/// </summary>
/// <typeparam name="TRequest">Command/Query which should be associated with only one use-case</typeparam>
/// <typeparam name="TResult">Output type</typeparam>
public interface IUseCase<in TRequest, out TResult> where TRequest : IRequest<TResult>
{
    TResult Execute(TRequest request);
}

/// <summary>
/// <inheritdoc cref="IRequest{TRequestResult}"/>
/// <para>
///     Request type with asynchronous result
/// </para>
/// </summary>
/// <typeparam name="TRequestResult"></typeparam>
public interface IAsyncRequest<TRequestResult> : IRequest<Task<TRequestResult>>
{
}

/// <summary>
/// <inheritdoc cref="IUseCase{TRequest,TResult}"/>
/// <para>
///     Represents use-case which should be executes asynchronously
/// </para>   
/// </summary>
public interface IAsyncUseCase<in TRequest, TResult> : IUseCase<TRequest, Task<TResult>>
    where TRequest : IAsyncRequest<TResult>
{
}