using Backend.Features.Order.Domain;
using Backend.Features.Order.Domain.ValueTypes;
using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Backend.Features.Order.Commands;

public sealed record ChangeStatusCommand(ulong OrderNumber, Guid UserId, OrderStatus Status)
    : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IChangeOrderStatus : IRequestHandlerWithException<ChangeStatusCommand, EmptyResult>
{
}