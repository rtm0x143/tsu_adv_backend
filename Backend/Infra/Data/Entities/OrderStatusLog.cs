using System.ComponentModel.DataAnnotations.Schema;
using Backend.Features.Order.Domain;
using Backend.Features.Order.Domain.ValueTypes;

namespace Backend.Infra.Data.Entities;

public class OrderStatusLog
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    /// <summary>
    /// Id of user who changed status
    /// </summary>
    public Guid UserId { get; set; }
    public string? Details { get; set; }
    public DateTime CreatedTime { get; set; }

    [ForeignKey(nameof(Order))] public ulong OrderNumber { get; set; }
    public Order Order { get; set; } = default!;
}