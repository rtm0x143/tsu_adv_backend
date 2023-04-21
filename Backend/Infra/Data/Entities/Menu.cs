namespace Backend.Infra.Data.Entities;

public class Menu
{
    public string Name { get; set; } = default!;
    public Guid RestaurantId { get; set; }
    public List<Dish>? Dishes { get; set; }
}