using Backend.Common.Dtos;
using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Backend.Features.Dish.Commands;

public sealed record ChangeDishCommand(Guid DishId, DishCreationDto Dish, Guid RestaurantId)
    : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IChangeDish : IRequestHandlerWithException<ChangeDishCommand, EmptyResult>
{
}