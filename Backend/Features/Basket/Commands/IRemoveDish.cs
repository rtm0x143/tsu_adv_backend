using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Backend.Features.Basket.Commands;

/// <summary>
/// Decreases count of dishes in basket by <paramref name="Count"/>
/// or removes it completely if count in basket less or equal <paramref name="Count"/>
/// </summary>
public sealed record RemoveDishCommand(Guid UserId, Guid DishId, ulong Count = 1)
    : IRequestWithException<EmptyResult, KeyNotFoundException>
{
    public static RemoveDishCommand RemoveAll(Guid userId, Guid dishId) => new(userId, dishId, ulong.MaxValue);
}

[RequestHandlerInterface]
public interface IRemoveDish : IRequestHandlerWithException<RemoveDishCommand, EmptyResult, KeyNotFoundException>
{
}