using System.Text.Json;
using System.Text.Json.Serialization;
using Backend.Common.Dtos;

namespace Backend.Converters;

public class JsonOrderNumberConverter : JsonConverter<OrderNumber>
{
    public override OrderNumber Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.GetString() is not string value)
            throw new JsonException($"Invalid json token in position {reader.Position.GetInteger()}");

        return new OrderNumber(
            OrderNumberFormatter.TryDecode(value, out var number) ? number : default,
            value);
    }

    public override void Write(Utf8JsonWriter writer, OrderNumber value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.Base32String);
}