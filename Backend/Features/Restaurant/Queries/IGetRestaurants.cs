using Backend.Common.Dtos;
using Common.App.Attributes;
using Common.App.RequestHandlers;

namespace Backend.Features.Restaurant.Queries;


public sealed record GetRestaurantsQuery(PaginationInfo<Guid> Pagination) : IRequest<RestaurantDto[]>;

[RequestHandlerInterface]
public interface IGetRestaurants : IRequestHandler<GetRestaurantsQuery, RestaurantDto[]>
{
}