using Common.App.Attributes;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Backend.Features.Menu.Commands;

public sealed record RemoveDishCommand(Guid RestaurantId, string MenuName, Guid DishId)
    : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IRemoveDish : IRequestHandlerWithException<RemoveDishCommand, EmptyResult>
{
}