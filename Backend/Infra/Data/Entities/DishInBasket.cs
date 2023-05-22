namespace Backend.Infra.Data.Entities;

public class DishInBasket
{
    public Guid UserId { get; set; }
    public Dish Dish { get; set; } = default!;
    public Guid DishId { get; set; }
    
    public ulong Count { get; set; }
}