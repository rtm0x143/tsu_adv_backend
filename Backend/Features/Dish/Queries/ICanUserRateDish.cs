using Common.App.Attributes;
using Common.App.RequestHandlers;

namespace Backend.Features.Dish.Queries;

public sealed record CanUserRateDishQuery(Guid DishId, Guid UserId) : IRequestWithException<bool, KeyNotFoundException>;

[RequestHandlerInterface]
public interface ICanUserRateDish : IRequestHandlerWithException<CanUserRateDishQuery, bool, KeyNotFoundException>
{
}