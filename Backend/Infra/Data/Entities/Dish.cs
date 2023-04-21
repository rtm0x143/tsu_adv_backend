using System.ComponentModel.DataAnnotations;
using Backend.Infra.Data.Enums;

namespace Backend.Infra.Data.Entities;

public class Dish
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    [Url] public string? Photo { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public DishCategory Category { get; set; }
    public bool IsVegetarian { get; set; }

    public List<Menu>? IntoMenus { get; set; }
    public Guid RestaurantId { get; set; }
}