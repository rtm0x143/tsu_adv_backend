using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Infra.Data.Entities;

public class DishInCart
{
    public Guid UserId { get; set; }
    public Dish Dish { get; set; } = default!;
    public Guid DishId { get; set; }
    
    public uint Count { get; set; }
}