using Backend.Features.Restaurant.Common;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.RequestHandlers;
using Common.Domain.Exceptions;
using Common.Domain.ValueTypes;

namespace Backend.Features.Restaurant.Commands;

public sealed record CreateRestaurantCommand(RestaurantCreationDto Restaurant) 
    : IRequestWithException<IdResult, CollisionException>;

[RequestHandlerInterface]
public interface ICreateRestaurant : IRequestHandlerWithException<CreateRestaurantCommand, IdResult, CollisionException>
{
}