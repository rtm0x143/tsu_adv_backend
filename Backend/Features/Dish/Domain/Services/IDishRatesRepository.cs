using Common.Domain;

namespace Backend.Features.Dish.Domain.Services;

public interface IDishRatesRepository : IRepository<DishRate>
{
    /// <summary>
    /// Calculates count of <paramref name="dish"/>'s rates
    /// </summary>
    Task<ulong> Count(Dish dish);

    /// <summary>
    /// Try get users with <paramref name="userId"/> rate on <paramref name="dish"/>
    /// </summary>
    /// <returns><see cref="DishRate"/> model or <c>null</c> if not found</returns>
    ValueTask<DishRate?> GetByDishAndUser(Dish dish, Guid userId);
}

