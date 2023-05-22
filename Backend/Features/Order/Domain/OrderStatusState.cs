using System.Diagnostics.CodeAnalysis;
using Backend.Features.Order.Domain.Exceptions;
using Backend.Features.Order.Domain.ValueTypes;
using Common.Domain.Exceptions;
using OneOf;

namespace Backend.Features.Order.Domain;

public partial class OrderStatusState
{
    public ulong OrderNumber { get; private set; }
    public OrderStatus OrderStatus { get; private set; }

    private List<OrderStatusLog> _orderStatusLogs = default!;
    public IReadOnlyCollection<OrderStatusLog> OrderStatusLogs => _orderStatusLogs;

    private bool _validateStatusChange(OrderStatus newStatus, [NotNullWhen(false)] out Exception? exception)
    {
        bool isValid = OrderStatus switch
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
            $"Can't change status of order({OrderNumber}) from {OrderStatus} to {newStatus}");
        return false;
    }

    public OneOf<OrderStatusLog, Exception> ChangeStatus(OrderStatus status, Guid interactingUserId)
    {
        if (!_validateStatusChange(status, out var exception)) return exception;

        var orderStatusLog = new OrderStatusLog(OrderNumber, interactingUserId, status,
            $"Previous status: {Enum.GetName(OrderStatus)};\nNew status: {Enum.GetName(status)};");
        _orderStatusLogs.Add(orderStatusLog);
        OrderStatus = orderStatusLog.Status;
        return orderStatusLog;
    }

    /// <exception cref="ArgumentException">When <paramref name="logs"/> is empty</exception>
    /// <exception cref="ConflictException">When some logs not related to this order</exception>
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
            exception = new ConflictException($"Some '{nameof(logs)}' items aren't related to this order");
            return false;
        } while (enumerator.MoveNext());

        exception = null;
        return true;
    }

    public static OneOf<OrderStatusState, Exception> Construct(ulong orderNumber, IEnumerable<OrderStatusLog> logs)
    {
        var orderStatusLogs = logs.ToList();
        if (!_validateStatusLogs(orderStatusLogs, orderNumber, out var exception)) return exception;
        orderStatusLogs.Sort();
        return new OrderStatusState
        {
            OrderNumber = orderNumber,
            OrderStatus = orderStatusLogs[^1].Status,
            _orderStatusLogs = orderStatusLogs
        };
    }
}