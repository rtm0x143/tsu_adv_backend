using Backend.Features.Restaurant.Common;
using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Backend.Features.Restaurant.Commands;

public sealed record ChangeRestaurantCommand(Guid RestaurantId, RestaurantCreationDto Restaurant) 
    : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IChangeRestaurant : IRequestHandlerWithException<ChangeRestaurantCommand, EmptyResult>
{
}