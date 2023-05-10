using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Backend.Features.Menu.Commands;

public sealed record AddDishCommand(Guid RestaurantId, string MenuName, Guid DishId)
    : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IAddDish : IRequestHandlerWithException<AddDishCommand, EmptyResult>
{
}

