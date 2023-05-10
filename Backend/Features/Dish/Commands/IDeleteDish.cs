using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Backend.Features.Dish.Commands;

public sealed record DeleteDishCommand(Guid RestaurantId, Guid DishId) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IDeleteDish : IRequestHandlerWithException<DeleteDishCommand, EmptyResult>
{
}