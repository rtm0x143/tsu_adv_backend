using Backend.Features.Order.Domain.ValueTypes;
using OneOf;

namespace Backend.Features.Order.Domain.Services;

public class OrderCreator
{
    private readonly IOrderNumberGenerator _numberGenerator;
    public OrderCreator(IOrderNumberGenerator numberGenerator) => _numberGenerator = numberGenerator;

    /// <summary>
    /// Creates brand new <see cref="Order"/> model
    /// </summary>
    /// <inheritdoc cref="DishInOrder.Construct"/>
    /// <inheritdoc cref="Order.Construct"/>
    public async Task<OneOf<Order, Exception>> CreateNew(
        DishInOrderDto[] dishes,
        string address,
        Guid userId,
        DateTime deliveryTime)
    {
        var number = await _numberGenerator.NextOrderNumber();
        var dishesInOrder = new List<DishInOrder>(dishes.Length);
        foreach (var dto in dishes)
        {
            var result = DishInOrder.Construct(number, dto.Dish, dto.Count);
            if (result.IsT1) return result.AsT1;
            dishesInOrder.Add(result.AsT0);
        }

        return Order.Construct(
            number: number,
            address: address,
            userId: userId,
            deliveryTime: deliveryTime,
            createdTime: DateTime.UtcNow,
            dishesInOrder: dishesInOrder,
            statusLogs: new[] { new OrderStatusLog(number, userId, OrderStatus.Created, "Created new order") });
    }

    /// <summary>
    /// Recreates specified order 
    /// </summary>
    /// <inheritdoc cref="Order.Construct"/>
    /// <returns>New <see cref="Order"/> model</returns>
    public async Task<OneOf<Order, Exception>> RepeatOrder(Order order, DateTime deliveryTime,
        string? newAddress = null, Guid newUserId = default)
    {
        newUserId = newUserId != default ? newUserId : order.UserId;
        newAddress ??= order.Address;
        var number = await _numberGenerator.NextOrderNumber();

        return Order.Construct(
            number: number,
            address: newAddress,
            userId: newUserId,
            deliveryTime: deliveryTime,
            createdTime: DateTime.UtcNow,
            dishesInOrder: order.Dishes.Select(inOrder => DishInOrder.Clone(inOrder, number)),
            statusLogs: new[]
            {
                new OrderStatusLog(number, newUserId, OrderStatus.Created, $"Recreated from order({order.Number})")
            });
    }
}