using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.RequestHandlers;
using Common.Domain.Exceptions;
using Common.Domain.ValueTypes;

namespace Backend.Features.Basket.Commands;

/// <summary>
/// Add dish to user's basket
/// </summary>
/// <exception cref="ActionFailedException">When <paramref name="UserId"/>'s basket contains dishes from different to <paramref name="DishId"/> restaurant</exception>
/// <exception cref="KeyNotFoundException"></exception>
public sealed record AddDishCommand(Guid UserId, Guid DishId, ulong Count) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IAddDish : IRequestHandlerWithException<AddDishCommand, EmptyResult>
{
}