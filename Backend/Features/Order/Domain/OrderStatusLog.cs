using Backend.Features.Order.Domain.ValueTypes;

namespace Backend.Features.Order.Domain;

public class OrderStatusLog : IComparable<OrderStatusLog>
{
    public Guid Id { get; private set; }
    public ulong OrderNumber { get; private set; }
    public Guid UserId { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedTime { get; private set; }
    public string? Details { get; private set; }

    public static OrderStatusLog Construct(Guid id, ulong orderNumber, Guid userId, OrderStatus status, string? details,
        DateTime createdTime) => new()
    {
        Id = id,
        OrderNumber = orderNumber,
        UserId = userId,
        Status = status,
        Details = details,
        CreatedTime = createdTime,
    };

    /// <summary>
    /// Creates new entity with new id
    /// </summary>
    public OrderStatusLog(ulong orderNumber, Guid userId, OrderStatus status, string? details, Guid id = default)
    {
        Id = id;
        OrderNumber = orderNumber;
        UserId = userId;
        Status = status;
        Details = details;
        CreatedTime = DateTime.UtcNow;
    }

    private OrderStatusLog()
    {
    }

    public int CompareTo(OrderStatusLog? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return CreatedTime.CompareTo(other.CreatedTime);
    }
}