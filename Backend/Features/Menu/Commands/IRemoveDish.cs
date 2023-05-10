using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Backend.Features.Menu.Commands;

public sealed record RemoveDishCommand(Guid RestaurantId, string MenuName, Guid DishId)
    : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IRemoveDish : IRequestHandlerWithException<RemoveDishCommand, EmptyResult>
{
}