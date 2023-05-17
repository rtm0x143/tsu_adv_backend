using Backend.Features.Order.Domain.Services;
using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Backend.Features.Order.Commands;

/// <remarks>See exceptions in <see cref="OrderCreator.CreateNew"/></remarks>
/// <returns>Numeric number of created order</returns>
public sealed record CreateOrderCommand(string Address, DateTime DeliveryTime, Guid UserId, bool ClearBasket)
    : IRequestWithException<IdResult<ulong>>;

[RequestHandlerInterface]
public interface ICreateOrder : IRequestHandlerWithException<CreateOrderCommand, IdResult<ulong>>
{
}