using Backend.Common.Dtos;
using Backend.Controllers;
using Backend.Infra.Data.Enums;
using Common.App.Attributes;
using Common.App.RequestHandlers;

namespace Backend.Features.Restaurant.Queries;

public sealed record GetRestaurantOrdersQuery(
    Guid RestaurantId,
    PaginationInfo<OrderNumber> Pagination,
    OrderSortingTypes SortingTypes = OrderSortingTypes.CreateTimeDec, 
    OrderStatus[]? InStatus = null) : IRequest<OrderShortDto[]>;

[RequestHandlerInterface]
public interface IGetRestaurantOrders : IRequestHandler<GetRestaurantOrdersQuery, OrderShortDto[]>
{
}