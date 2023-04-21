using Common.App.Attributes;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Backend.Features.Menu.Commands;

public sealed record AddDishCommand(Guid RestaurantId, string MenuName, Guid DishId)
    : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IAddDish : IRequestHandlerWithException<AddDishCommand, EmptyResult>
{
}

