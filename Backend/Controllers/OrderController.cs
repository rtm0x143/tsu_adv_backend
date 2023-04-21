using System.Text.Json;
using System.Text.Json.Serialization;
using Backend.Common;
using Backend.Common.Dtos;
using Backend.Infra.Data.Enums;
using Backend.Mappers;
using Common.App;
using Common.App.Models.Results;
using Common.App.RequestHandlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Backend.Controllers;

public class JsonBase32LongConverter : JsonConverter<OrderNumber>
{
    public override OrderNumber Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.GetString() is not string value)
            throw new JsonException($"Invalid json token in position {reader.Position.GetInteger()}");

        return new OrderNumber(
            Base32Converter.TryToLong(value, out var number) ? number : -1,
            value);
    }

    public override void Write(Utf8JsonWriter writer, OrderNumber value, JsonSerializerOptions options)
    {
        value.Base32String ??= Base32Converter.Encode(value.Numeric);
        writer.WriteStringValue(value.Base32String);
    }
}

[JsonConverter(typeof(JsonBase32LongConverter))]
public record struct OrderNumber(long Numeric, string? Base32String = null)
{
    public long Numeric = Numeric;
    public string? Base32String = Base32String;
}

public record OrderDto(OrderNumber Number, decimal Price, string Address, OrderStatus Status, DishShortDto[] Dishes)
    : OrderShortDto(Number, Price, Address, Status);

public enum OrderSortingTypes
{
    CreateTimeAsc,
    CreateTimeDec,
    DeliveryTimeAsc,
    DeliveryTimeDec
}

public sealed record GetOrdersQuery(
    PaginationInfo<OrderNumber> Pagination,
    Guid UserId = default,
    OrderSortingTypes SortingTypes = OrderSortingTypes.CreateTimeAsc,
    DateTime NotBefore = default,
    DateTime NotAfter = default) : IRequest<OrderShortDto[]>;

public sealed record GetProcessingOrdersQuery : IRequest<OrderShortDto[]>;

public class OrderController : CommonApiControllerBase<OrderController>
{
    [HttpGet("{number}")]
    public Task<ActionResult<OrderDto>> GetByNumber(
        [FromRoute, ModelBinder(typeof(Base32LongModelBinder))]
        OrderNumber number)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public Task<ActionResult<OrderShortDto[]>> Get([FromQuery] GetOrdersQuery query)
    {
        throw new NotImplementedException();
    }

    [HttpGet("processing")]
    public Task<ActionResult<OrderShortDto[]>> Get()
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public Task<ActionResult<IdResult>> Create([FromQuery] bool truncateBasket = true)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{number}/repeat")]
    public Task<ActionResult<IdResult>> Repeat(
        [FromRoute, ModelBinder(typeof(Base32LongModelBinder))]
        OrderNumber number)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{number}/status/{status}")]
    public Task<ActionResult<IdResult>> SetStatus(
        [FromRoute, ModelBinder(typeof(Base32LongModelBinder))]
        OrderNumber number, OrderStatus status)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public Task<ActionResult> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}