using Backend.Features.Order.Domain.ValueTypes;

namespace Backend.Features.Dish.Domain;

public class Order
{
    public required ulong Number { get; init; }
    public required OrderStatus Status { get; init; }
    public required Guid UserId { get; init; }
}

