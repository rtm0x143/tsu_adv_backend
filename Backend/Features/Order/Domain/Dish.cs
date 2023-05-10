namespace Backend.Features.Order.Domain;

public class Dish
{
    public Guid Id { get; private set; }
    public Guid RestaurantId { get; private set; }
    public decimal Price { get; private set; }
}