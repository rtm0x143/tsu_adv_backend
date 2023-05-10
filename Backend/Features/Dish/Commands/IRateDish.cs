using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Backend.Features.Dish.Commands;

public sealed record RateDishCommand(Guid DishId, float Score, Guid UserId) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IRateDish : IRequestHandlerWithException<RateDishCommand, EmptyResult>
{
}