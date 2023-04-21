using Backend.Infra.Data.Enums;

namespace Backend.Infra.Data.Entities;

public class OrderStatusLog
{
    public long Id { get; set; }
    public OrderStatus Status { get; set; }
    /// <summary>
    /// Id of user who changed status
    /// </summary>
    public Guid UserId { get; set; }
    public string? Details { get; set; }
    public Order Order { get; set; } = default!;
}