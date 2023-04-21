using Backend.Features.Restaurant.Common;
using Common.App.Attributes;
using Common.App.RequestHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Restaurant.Queries;

public sealed record GetRestaurantsQuery(PaginationInfo<Guid> Pagination) : IRequest<RestaurantDto[]>;

[RequestHandlerInterface]
public interface IGetRestaurants : IRequestHandler<GetRestaurantsQuery, RestaurantDto[]>
{
}