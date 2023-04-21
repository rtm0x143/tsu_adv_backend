using Common.App.Attributes;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Backend.Features.Restaurant.Commands;

public sealed record DeleteRestaurantCommand(Guid RestaurantId) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IDeleteRestaurant : IRequestHandlerWithException<DeleteRestaurantCommand, EmptyResult>
{
}