using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Features.Order.Domain;
using Backend.Features.Order.Domain.ValueTypes;

namespace Backend.Infra.Data.Entities;

public class Order
{
    [Key] public ulong Number { get; set; }
    public decimal Price { get; set; }
    public string Address { get; set; } = default!;
    public OrderStatus Status { get; set; }

    [ForeignKey(nameof(Restaurant))] public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    public List<DishInOrder>? Dishes { get; set; }

    public Guid UserId { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime DeliveryTime { get; set; }

    public List<OrderStatusLog>? StatusLogs { get; set; }
}