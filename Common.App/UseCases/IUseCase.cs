namespace Common.App.UseCases;

/// <summary>
/// Generic use-case interface
/// </summary>
/// <typeparam name="TRequest">Command/Query which should be associated with only one use-case</typeparam>
/// <typeparam name="TResult">Output type</typeparam>
public interface IUseCase<in TRequest, out TResult>
{
    TResult Execute(TRequest request);
}

public interface IAsyncUseCase<in TRequest, TResult> : IUseCase<TRequest, Task<TResult>>
{
}