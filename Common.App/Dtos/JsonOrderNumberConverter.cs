using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.App.Dtos;

public class JsonOrderNumberConverter : JsonConverter<OrderNumber>
{
    public override OrderNumber Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.GetString() is not string value)
            throw new JsonException($"Invalid json token in position {reader.Position.GetInteger()}");

        return new OrderNumber(OrderNumberFormatter.TryDecode(value, out var number) ? number : default);
    }

    public override void Write(Utf8JsonWriter writer, OrderNumber value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.String);
}