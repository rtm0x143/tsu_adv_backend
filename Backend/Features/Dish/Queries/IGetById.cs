using Common.App.Attributes;
using Common.App.RequestHandlers;

namespace Backend.Features.Dish.Queries;

public sealed record GetByIdQuery(Guid Id) : IRequestWithException<DishDto, KeyNotFoundException>;

[RequestHandlerInterface]
public interface IGetById : IRequestHandlerWithException<GetByIdQuery, DishDto, KeyNotFoundException>
{
}