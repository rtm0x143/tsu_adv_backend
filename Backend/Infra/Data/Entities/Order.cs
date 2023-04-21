using System.ComponentModel.DataAnnotations;
using Backend.Infra.Data.Enums;

namespace Backend.Infra.Data.Entities;

public class Order
{
    [Key] public long Number { get; set; }
    public decimal Price { get; set; }
    public string Address { get; set; } = default!;
    public OrderStatus Status { get; set; }

    public Restaurant Restaurant { get; set; }
    public List<DishInOrder>? Dishes { get; set; }

    public Guid UserId { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime DeliveryTime { get; set; }

    public List<OrderStatusLog>? StatusLogs { get; set; }
}