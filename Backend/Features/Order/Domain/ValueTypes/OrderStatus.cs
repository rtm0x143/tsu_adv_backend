using System.Text.Json.Serialization;

namespace Backend.Features.Order.Domain.ValueTypes;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    Created,
    Kitchen,
    Packaging,
    Delivery,
    Delivered,
    Canceled
}

public static class OrderStatuses
{
    public static OrderStatus[] ProcessingOrderStatuses =
    {
        OrderStatus.Created, OrderStatus.Kitchen, OrderStatus.Packaging, OrderStatus.Delivery
    };
}