using Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace Common.Infra.Dal;

public class RepositoryBase<TEntity> : IRepository<TEntity>
{
    protected virtual IQueryable<TEntity> QuerySource { get; }
    public RepositoryBase(IQueryable<TEntity> queryable) => QuerySource = queryable;

    public Task<TResult[]> QueryMany<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query) =>
        query.Invoke(QuerySource).ToArrayAsync();

    public Task<TResult?> QueryOne<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query) =>
        query.Invoke(QuerySource).FirstOrDefaultAsync();
}