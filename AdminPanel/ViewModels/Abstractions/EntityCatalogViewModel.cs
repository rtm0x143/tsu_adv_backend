using AdminPanel.Views.Shared.DisplayTemplates;

namespace AdminPanel.ViewModels.Abstractions;

public abstract class EntityCatalogViewModel
{
    public object? Query { get; set; }
    public object[] Entities { get; set; } = Array.Empty<object>();

    public PropertyTableViewData? PropertyTableViewData { get; set; }

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