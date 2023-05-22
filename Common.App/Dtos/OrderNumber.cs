using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Common.App.Dtos;

[JsonConverter(typeof(JsonOrderNumberConverter))]
[ModelBinder(typeof(OrderNumberModelBinder))]
public record struct OrderNumber(ulong Numeric, string? String = null)
{
    public ulong Numeric = Numeric;
    public string String = String ?? OrderNumberFormatter.Encode(Numeric);

    public static implicit operator OrderNumber(ulong number) => new(number);
    public static implicit operator ulong(OrderNumber orderNumber) => orderNumber.Numeric;
}