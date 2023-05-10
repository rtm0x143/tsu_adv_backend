using System.Text.Json.Serialization;
using Backend.Converters;
using Backend.Features.Order.Domain;
using Backend.Features.Order.Domain.ValueTypes;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Common.Dtos;

[JsonConverter(typeof(JsonOrderNumberConverter))]
[ModelBinder(typeof(OrderNumberModelBinder))]
public record struct OrderNumber(ulong Numeric, string? Base32String = null)
{
    public ulong Numeric = Numeric;
    public string Base32String = Base32String ?? OrderNumberFormatter.Encode(Numeric);

    public static implicit operator OrderNumber(ulong number) => new(number);
}

public record OrderShortDto(OrderNumber Number, decimal Price, string Address, OrderStatus Status,
    DateTime CreatedTime, DateTime DeliveryTime);

public record OrderDto(OrderNumber Number, decimal Price, string Address, OrderStatus Status,
        DateTime CreatedTime, DateTime DeliveryTime,
        DishShortDto[] Dishes, Guid UserId, RestaurantDto Restaurant)
    : OrderShortDto(Number, Price, Address, Status, CreatedTime, DeliveryTime);