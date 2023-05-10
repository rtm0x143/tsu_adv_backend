using Backend.Features.Dish.Domain.ValueTypes;
using Common.App.Utils;
using OneOf;

namespace Backend.Features.Dish.Domain.Services;

public class RatingService
{
    /// <summary>
    /// Indicated how much rates count should change to trigger rate value recalculation
    /// </summary>
    public double RecalculateVariance = .01d;

    private readonly IDishRatesRepository _repository;
    public RatingService(IDishRatesRepository repository) => _repository = repository;

    private Task<OneOf<Dish, ArgumentOutOfRangeException>> RecalculateRate(Dish dish, ulong count)
    {
        return _repository.QueryMany(query =>
                query.Where(rate => rate.DishId == dish.Id)
                    .Select(rate => rate.Score.Value))
            .ContinueWith(t => RateScore.Construct(t.Result.Average())
                .MapT0(rate =>
                {
                    dish.CachedRate = new Rate(rate, count);
                    return dish;
                }));
    }

    private record struct RateRecalculationNeededResult(bool IsNeeded, ulong Count);

    private Task<RateRecalculationNeededResult> IsRateRecalculationNeeded(Dish dish)
    {
        return _repository.Count(dish).ContinueWith(t => new RateRecalculationNeededResult(
            IsNeeded: Math.Abs((double)dish.CachedRate.Count - t.Result)
                      > dish.CachedRate.Count * RecalculateVariance,
            Count: t.Result));
    }

    public async Task<OneOf<Dish, ArgumentOutOfRangeException>> RecalculateRate(Dish dish) =>
        await RecalculateRate(dish, await _repository.Count(dish));

    /// <summary>
    /// Set user's rate to dish 
    /// </summary>
    /// <returns><see cref="DishRate"/> model or exception</returns>
    public async Task<OneOf<DishRate, ArgumentOutOfRangeException>> RateDish(Dish dish, RateScore score, Guid userId)
    {
        if (await _repository.GetByDishAndUser(dish, userId) is not DishRate dishRate)
            dishRate = new DishRate { DishId = dish.Id, Score = score, UserId = userId };
        else dishRate.Score = score;

        if (await IsRateRecalculationNeeded(dish) is { IsNeeded: true } isNeededResult)
        {
            var recalculateResult = await RecalculateRate(dish, isNeededResult.Count);
            if (!recalculateResult.Succeeded()) recalculateResult.Error();
        }

        return dishRate;
    }
}