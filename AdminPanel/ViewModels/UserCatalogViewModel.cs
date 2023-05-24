using AdminPanel.Entities;
using AdminPanel.ViewModels.Abstractions;
using Common.App.Dtos;

namespace AdminPanel.ViewModels;

public class UsersQuery : IPaging<Guid>
{
    public PaginationInfo<Guid> Pagination { get; set; }
    public string? InRole { get; set; }
    public Guid? InRestaurant { get; set; }
}

public class UserCatalogViewModel : EntityCatalogWithPagination<UserPlainObject, UsersQuery, Guid>
{
    protected override Guid GetEntityId(UserPlainObject entity) => entity.Id;
}