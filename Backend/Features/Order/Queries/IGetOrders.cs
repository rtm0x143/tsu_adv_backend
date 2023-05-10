using System.Text.Json.Serialization;
using Backend.Common.Dtos;
using Backend.Features.Order.Domain;
using Backend.Features.Order.Domain.ValueTypes;
using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Infra.Dal;

namespace Backend.Features.Order.Queries;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderSortFactors
{
    CreateTime,
    DeliveryTime
}

public sealed record GetOrdersQuery(
    PaginationInfo<OrderNumber?> Pagination,
    Guid RestaurantId = default,
    Guid UserId = default,
    SortType SortType = SortType.Asc,
    OrderSortFactors SortFactor = OrderSortFactors.CreateTime,
    OrderStatus[]? InStatus = null,
    DateTime NotBefore = default,
    DateTime NotAfter = default,
    bool FilterByDeliveryTime = false) : IRequest<OrderShortDto[]>;

[RequestHandlerInterface]
public interface IGetOrders : IRequestHandler<GetOrdersQuery, OrderShortDto[]>
{
}