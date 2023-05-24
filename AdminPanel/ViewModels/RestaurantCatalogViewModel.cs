using AdminPanel.Entities;
using AdminPanel.ViewModels.Abstractions;
using Common.App.Dtos;

namespace AdminPanel.ViewModels;

public class RestaurantQuery : IPaging<Guid>
{
    public PaginationInfo<Guid> Pagination { get; set; }
}

public class RestaurantCatalogViewModel : EntityCatalogWithPagination<Restaurant, RestaurantQuery, Guid>
{
    protected override Guid GetEntityId(Restaurant entity) => entity.Id;
}