using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Backend.Features.Basket.Commands;

/// <summary>
/// Add dish to user's basket
/// </summary>
/// <exception cref="ActionFailedException">When <paramref name="UserId"/>'s basket contains dishes from different to <paramref name="DishId"/> restaurant</exception>
/// <exception cref="KeyNotFoundException"></exception>
public sealed record AddDishCommand(Guid UserId, Guid DishId) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IAddDish : IRequestHandlerWithException<AddDishCommand, EmptyResult>
{
}