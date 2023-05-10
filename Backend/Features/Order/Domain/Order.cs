using System.Diagnostics.CodeAnalysis;
using Backend.Features.Order.Domain.Exceptions;
using Backend.Features.Order.Domain.ValueTypes;
using Common.App.Exceptions;
using Common.Domain.Exceptions;
using OneOf;

namespace Backend.Features.Order.Domain;

public record struct DishInOrderDto(Dish Dish, uint Count);

public partial class Order
{
    public ulong Number { get; private set; }
    public decimal Price { get; private set; }
    public string Address { get; private set; } = default!;
    public OrderStatus Status { get; private set; }

    public Guid RestaurantId { get; private set; }

    private List<DishInOrder> _dishes = null!;
    public IReadOnlyCollection<DishInOrder> Dishes => _dishes;

    public Guid UserId { get; private set; }
    public DateTime CreatedTime { get; private set; }
    public DateTime DeliveryTime { get; private set; }

    private List<OrderStatusLog> _statusLogs = null!;
    public IReadOnlyCollection<OrderStatusLog> StatusLogs => _statusLogs;

    /// <exception cref="EmptyOrderException">When <paramref name="dishInOrders"/> is empty</exception>
    /// <exception cref="DifferentRestaurantException">When <paramref name="dishInOrders"/> contain dishes from different restaurants</exception>
    private static bool _validateDishesInOrder(IEnumerable<DishInOrder> dishInOrders,
        [NotNullWhen(false)] out Exception? exception)
    {
        using var enumerator = dishInOrders.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            exception = new EmptyOrderException();
            return false;
        }

        var restaurantId = enumerator.Current.Dish.RestaurantId;

        do
        {
            if (enumerator.Current.Dish.RestaurantId == restaurantId) continue;
            exception = new DifferentRestaurantException();
            return false;
        } while (enumerator.MoveNext());

        exception = null;
        return true;
    }

    /// <exception cref="ArgumentException">When <paramref name="logs"/> is empty</exception>
    /// <exception cref="UnsuitableDataException">When some logs not related to this order</exception>
    private static bool _validateStatusLogs(IEnumerable<OrderStatusLog> logs, ulong orderNumber,
        [NotNullWhen(false)] out Exception? exception)
    {
        using var enumerator = logs.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            exception = new ArgumentException("Collection was empty", nameof(logs));
            return false;
        }

        do
        {
            if (enumerator.Current.OrderNumber == orderNumber) continue;
            exception = new UnsuitableDataException($"Some '{nameof(logs)}' items aren't related to this order");
            return false;
        } while (enumerator.MoveNext());

        exception = null;
        return true;
    }

    /// <summary>
    /// Constructs existing <see cref="Order"/> model 
    /// </summary>
    /// <inheritdoc cref="_validateStatusLogs"/>
    /// <inheritdoc cref="_validateDishesInOrder"/>
    /// <exception cref="ArgumentOutOfRangeException">When <paramref name="deliveryTime"/> less than <paramref name="createdTime"/></exception>
    /// <exception cref="HadDefaultValueException"></exception>
    /// <returns><see cref="Order"/> instance or exception from 'Exceptions' section</returns>
    public static OneOf<Order, Exception> Construct(
        ulong number,
        string address,
        Guid userId,
        DateTime createdTime,
        DateTime deliveryTime,
        IEnumerable<DishInOrder> dishesInOrder,
        IEnumerable<OrderStatusLog> statusLogs)
    {
        if (userId == default) return new HadDefaultValueException(nameof(userId));
        if (createdTime == default) return new HadDefaultValueException(nameof(createdTime));
        if (createdTime == default) return new HadDefaultValueException(nameof(createdTime));

        var dishes = dishesInOrder.ToList();
        if (!_validateDishesInOrder(dishes, out var dishesException)) return dishesException;

        var logs = statusLogs.ToList();
        if (!_validateStatusLogs(logs, number, out var logsException)) return logsException;

        if (deliveryTime < createdTime)
            return new ArgumentOutOfRangeException(
                nameof(deliveryTime), "Delivery time can't be less than create time");

        return new Order
        {
            Number = number,
            Address = address,
            UserId = userId,
            RestaurantId = dishes[0].Dish.RestaurantId,
            DeliveryTime = deliveryTime,
            CreatedTime = createdTime,
            Price = dishes.Aggregate(new decimal(), (sum, inOrder) => sum + inOrder.Dish.Price * inOrder.Count),
            Status = logs.Max()!.Status,
            _dishes = dishes,
            _statusLogs = logs
        };
    }

    /// <exception cref="InvalidOrderFlowException"></exception>
    private bool _validateStatusChange(OrderStatus newStatus, [NotNullWhen(false)] out Exception? exception)
    {
        bool isValid = Status switch
        {
            OrderStatus.Canceled => newStatus == OrderStatus.Created,
            OrderStatus.Created => new[] { OrderStatus.Canceled, OrderStatus.Kitchen }.Contains(newStatus),
            OrderStatus.Kitchen => newStatus == OrderStatus.Packaging,
            OrderStatus.Packaging => newStatus == OrderStatus.Delivery,
            OrderStatus.Delivery => new[] { OrderStatus.Canceled, OrderStatus.Delivered }.Contains(newStatus),
            OrderStatus.Delivered => false,
            _ => false
        };

        if (isValid)
        {
            exception = null;
            return true;
        }

        exception = new InvalidOrderFlowException(
            $"Can't change status of order({Number}) from {Status} to {newStatus}");
        return false;
    }

    /// <inheritdoc cref="_validateStatusChange"/>
    public OneOf<OrderStatusLog, Exception> ChangeStatus(OrderStatus status, Guid interactingUserId)
    {
        if (!_validateStatusChange(status, out var exception)) return exception;

        var orderStatusLog = new OrderStatusLog(Number, interactingUserId, status,
            $"Previous status: {Enum.GetName(Status)!};\nNew status: {Enum.GetName(status)};");
        _statusLogs.Add(orderStatusLog);
        Status = status;
        return orderStatusLog;
    }
}