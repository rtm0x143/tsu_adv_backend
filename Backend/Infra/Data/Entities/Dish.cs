using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Features.Dish.Domain;
using Backend.Features.Dish.Domain.ValueTypes;

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

    [ForeignKey(nameof(Restaurant))] public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = default!;

    public class Rate
    {
        public float Score;
        public ulong Count;
    }

    public Rate CachedRate { get; set; } = null!;
}