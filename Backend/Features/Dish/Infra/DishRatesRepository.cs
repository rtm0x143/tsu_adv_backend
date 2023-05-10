using Backend.Features.Dish.Domain.Services;
using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Dish.Infra;

public class DishRatesRepository : RepositoryBase<Domain.DishRate>, IDishRatesRepository
{
    private readonly DbSet<Domain.DishRate> _set;
    public DishRatesRepository(DishDbContext context) : base(context.DishRates) => _set = context.DishRates;

    public Task<ulong> Count(Domain.Dish dish) =>
        _set.Where(rate => rate.DishId == dish.Id)
            .LongCountAsync()
            .ContinueWith(t => (ulong)t.Result);

    public ValueTask<Domain.DishRate?> GetByDishAndUser(Domain.Dish dish, Guid userId) =>
        _set.FindAsync(dish.Id, userId);
}