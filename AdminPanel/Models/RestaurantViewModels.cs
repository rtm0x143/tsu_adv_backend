using Common.App.Dtos;

// TODO:

namespace AdminPanel.Models;

public class RestaurantQuery
{
    public PaginationInfo<Guid>? Pagination { get; set; }
}

public record Restaurant(Guid Id, string Name);

public abstract class EntityCatalogViewModel
{
    public object? Query { get; set; }
    public object[] Entities { get; set; } = Array.Empty<object>();

    public abstract Type QueryType { get; }
    public abstract Type EntityType { get; }
}

public class EntityCatalogViewModel<TEntity, TQuery> : EntityCatalogViewModel where TEntity : class where TQuery : class
{
    public new virtual TQuery? Query
    {
        get => (TQuery?)base.Query;
        set => base.Query = value;
    }

    public new virtual TEntity[]? Entities
    {
        get => base.Entities as TEntity[];
        set => base.Entities = value as object[] ?? Array.Empty<object>();
    }

    public override Type QueryType => typeof(TQuery);
    public override Type EntityType => typeof(TEntity);
}

public class RestaurantCatalogViewModel : EntityCatalogViewModel<Restaurant, RestaurantQuery>
{
    public override Restaurant[]? Entities
    {
        set
        {
            base.Entities = value;
            if (value == null) return;
            Query ??= new RestaurantQuery();
            Query.Pagination =
                new PaginationInfo<Guid>(value.Length, value.LastOrDefault()?.Id ?? default);
        }
    }
}