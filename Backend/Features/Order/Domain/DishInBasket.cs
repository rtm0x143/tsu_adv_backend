namespace Backend.Features.Order.Domain;

public class DishInBasket
{
    public Guid UserId { get; private set; }
    public Dish Dish { get; private set; } = default!;
    public Guid DishId { get; private set; }
    public ulong Count { get; private set; }
}