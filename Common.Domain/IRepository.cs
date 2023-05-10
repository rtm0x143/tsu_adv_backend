namespace Common.Domain;

/// <summary>
/// An generic repository interface
/// </summary>
/// <typeparam name="TEntity">Target entity type</typeparam>
public interface IRepository<out TEntity>
{
    /// <summary>
    /// Retrieve entities with some specific query 
    /// </summary>
    Task<TResult[]> QueryMany<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query);

    /// <summary>
    /// Retrieve only one entity with some specific query 
    /// </summary>
    /// <returns>Result or default if no entities satisfy query</returns>
    Task<TResult?> QueryOne<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query);
}