using Backend.Common.Dtos;
using Backend.Features.Order.Domain.Services;
using Common.App.Attributes;
using Common.App.RequestHandlers;

namespace Backend.Features.Order.Commands;

public record OrderCreateResult(OrderNumber OrderNumber);

/// <remarks>See exceptions in <see cref="OrderCreator.CreateNew"/></remarks>
public sealed record CreateOrderCommand(string Address, DateTime DeliveryTime, Guid UserId, bool ClearBasket)
    : IRequestWithException<OrderCreateResult>;

[RequestHandlerInterface]
public interface ICreateOrder : IRequestHandlerWithException<CreateOrderCommand, OrderCreateResult>
{
}