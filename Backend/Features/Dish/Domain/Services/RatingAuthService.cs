using Backend.Features.Order.Domain.ValueTypes;
using Common.Domain;
using Common.Domain.Exceptions;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace Backend.Features.Dish.Domain.Services;

public class RatingAuthService
{
    private readonly IRepository<Dish> _dishRepository;
    private readonly IRepository<DishInOrder> _inOrderRepository;

    public RatingAuthService(IRepository<Dish> dishRepository, IRepository<DishInOrder> inOrderRepository)
    {
        _dishRepository = dishRepository;
        _inOrderRepository = inOrderRepository;
    }

    /// <summary>
    /// Check if some user can rate specified dish 
    /// </summary>
    public async Task<OneOf<EmptyResult, NotPermittedException, KeyNotFoundException>> CanUserRateDish(
        Guid dishId, Guid userId)
    {
        if (await _dishRepository.QueryOne(query => query.Where(dish => dish.Id == dishId)) == null)
            return new KeyNotFoundException(nameof(dishId));

        var inOrder = await _inOrderRepository.QueryOne(query => query.Where(
                inOrder => inOrder.DishId == dishId
                           && inOrder.Order.UserId == userId
                           && inOrder.Order.Status == OrderStatus.Delivered))
            .ConfigureAwait(false);

        return inOrder == null
            ? new NotPermittedException("Specified user never successfully ordered that dish")
            : new EmptyResult();
    }
}