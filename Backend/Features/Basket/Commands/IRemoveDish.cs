using Common.App.Attributes;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Backend.Features.Basket.Commands;

/// <summary>
/// Decreases count of dishes in basket by <paramref name="Count"/>
/// or removes it completely if count in basket less or equal <paramref name="Count"/>
/// </summary>
public sealed record RemoveDishCommand(Guid UserId, Guid DishId, uint Count = 1)
    : IRequestWithException<EmptyResult, KeyNotFoundException>
{
    public static RemoveDishCommand RemoveAll(Guid userId, Guid dishId) => new(userId, dishId, uint.MaxValue);
}

[RequestHandlerInterface]
public interface IRemoveDish : IRequestHandlerWithException<RemoveDishCommand, EmptyResult, KeyNotFoundException>
{
}