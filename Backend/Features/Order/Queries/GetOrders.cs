using Backend.Common.Dtos;
using Backend.Converters;
using Backend.Infra.Data;
using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Order.Queries;

public class GetOrders : IGetOrders
{
    private readonly BackendDbContext _context;
    public GetOrders(BackendDbContext context) => _context = context;

    public Task<OrderShortDto[]> Execute(GetOrdersQuery query)
    {
        IQueryable<Entities.Order> queryable = _context.Orders;

        if (query.UserId != default)
            queryable = queryable.Where(order => order.UserId == query.UserId);

        if (query.RestaurantId != default)
            queryable = queryable.Where(order => order.RestaurantId == query.RestaurantId);

        if (query.NotBefore != default)
            queryable = query.FilterByDeliveryTime
                ? queryable.Where(order => order.DeliveryTime > query.NotBefore)
                : queryable.Where(order => order.CreatedTime > query.NotBefore);

        if (query.NotAfter != default)
            queryable = query.FilterByDeliveryTime
                ? queryable.Where(order => order.DeliveryTime > query.NotAfter)
                : queryable.Where(order => order.CreatedTime > query.NotAfter);

        if (!query.InStatus.IsNullOrEmpty())
            queryable = queryable.Where(o => query.InStatus!.Contains(o.Status));

        queryable = query.SortFactor switch
        {
            OrderSortFactors.DeliveryTime => queryable.OrderBy(order => order.DeliveryTime, query.SortType),
            _ => queryable.OrderBy(order => order.CreatedTime, query.SortType)
        };

        if (query.Pagination.AfterRecord != null)
            queryable = queryable.Where(order => order.Number > query.Pagination.AfterRecord.Value.Numeric);

        return queryable.Take(query.Pagination.PageSize)
            .Select(OrderMapper.ProjectToShortDto)
            .AsNoTracking()
            .ToArrayAsync();
    }
}