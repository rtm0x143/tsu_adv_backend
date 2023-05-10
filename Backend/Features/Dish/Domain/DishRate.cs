using Backend.Features.Dish.Domain.ValueTypes;

namespace Backend.Features.Dish.Domain;

public class DishRate
{
    public required Guid DishId { get; set; }
    public required Guid UserId { get; set; }
    public required RateScore Score { get; set; }
}