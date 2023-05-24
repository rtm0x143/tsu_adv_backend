using Common.App.Dtos;

namespace AdminPanel.ViewModels.Abstractions;

public interface IPaging<TId>
{
    public PaginationInfo<TId> Pagination { get; set; }
}

public abstract class EntityCatalogWithPagination<TEntity, TQuery, TId> : EntityCatalogViewModel<TEntity, TQuery>
    where TEntity : class
    where TQuery : class, IPaging<TId>

{
    protected abstract TId GetEntityId(TEntity entity);

    public override TEntity[]? Entities
    {
        set
        {
            base.Entities = value;
            if (value == null)
            {
                if (Query?.Pagination != null) Query.Pagination = Query.Pagination with { AfterRecord = default };
                return;
            }

            if (Query != null && value.Length > 0)
                Query.Pagination = new PaginationInfo<TId>(value.Length, GetEntityId(value[^1]));
        }
    }
}
