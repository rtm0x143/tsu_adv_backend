using System.Text.Json.Serialization;

namespace Backend.Infra.Data.Enums;

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