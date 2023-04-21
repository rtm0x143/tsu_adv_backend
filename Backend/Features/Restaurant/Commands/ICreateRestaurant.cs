using Backend.Features.Restaurant.Common;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Backend.Features.Restaurant.Commands;

public sealed record CreateRestaurantCommand(RestaurantCreationDto Restaurant) 
    : IRequestWithException<IdResult, CollisionException>;

[RequestHandlerInterface]
public interface ICreateRestaurant : IRequestHandlerWithException<CreateRestaurantCommand, IdResult, CollisionException>
{
}