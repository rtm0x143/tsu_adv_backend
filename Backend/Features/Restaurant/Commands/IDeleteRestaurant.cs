using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Backend.Features.Restaurant.Commands;

public sealed record DeleteRestaurantCommand(Guid RestaurantId) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IDeleteRestaurant : IRequestHandlerWithException<DeleteRestaurantCommand, EmptyResult>
{
}