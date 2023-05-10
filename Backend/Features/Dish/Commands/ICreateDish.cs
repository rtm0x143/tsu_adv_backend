using Backend.Common.Dtos;
using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Backend.Features.Dish.Commands;

public sealed record CreateDishCommand(DishCreationDto Dish, Guid RestaurantId) : IRequestWithException<IdResult>;

[RequestHandlerInterface]
public interface ICreateDish : IRequestHandlerWithException<CreateDishCommand, IdResult>
{
}