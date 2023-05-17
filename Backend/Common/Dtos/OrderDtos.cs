using Backend.Features.Order.Domain.ValueTypes;
using Common.App.Dtos;

namespace Backend.Common.Dtos;

public record OrderShortDto(OrderNumber Number, decimal Price, string Address, OrderStatus Status,
    DateTime CreatedTime, DateTime DeliveryTime);

public record OrderDto(OrderNumber Number, decimal Price, string Address, OrderStatus Status,
        DateTime CreatedTime, DateTime DeliveryTime,
        DishShortDto[] Dishes, Guid UserId, RestaurantDto Restaurant)
    : OrderShortDto(Number, Price, Address, Status, CreatedTime, DeliveryTime);