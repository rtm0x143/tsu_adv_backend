using Backend.Common.Dtos;
using Common.App.Attributes;
using Common.App.RequestHandlers;

namespace Backend.Features.Restaurant.Queries;

public sealed record GetByIdQuery(Guid Id) : IRequestWithException<RestaurantDto, KeyNotFoundException>;

[RequestHandlerInterface]
public interface IGetById : IRequestHandlerWithException<GetByIdQuery, RestaurantDto, KeyNotFoundException>
{
}