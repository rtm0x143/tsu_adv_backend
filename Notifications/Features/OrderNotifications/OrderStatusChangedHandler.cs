using Backend.Messaging.Messages.Events;
using Common.App.Utils;
using Notifications.Domain.Services;
using Notifications.Features.Notifications.Commands;
using NServiceBus;

namespace Notifications.Features.OrderNotifications;

public class OrderStatusChangedHandler : IHandleMessages<OrderStatusChangedEvent>
{
    private readonly ICommitNotification _commitNotification;
    private readonly ILogger<OrderStatusChangedHandler> _logger;

    public OrderStatusChangedHandler(ICommitNotification commitNotification, ILogger<OrderStatusChangedHandler> logger)
    {
        _commitNotification = commitNotification;
        _logger = logger;
    }

    public async Task Handle(OrderStatusChangedEvent @event, IMessageHandlerContext context)
    {
        var notificationResult = new OrderNotificationCreator()
            .CreateNewStatusChanged(@event.OrderStatus, @event.OrderNumber.String, @event.Description);

        if (!notificationResult.Succeeded())
        {
            _logger.LogError(notificationResult.Error(), "Received event was invalid");
            return;
        }

        await _commitNotification.Execute(new(notificationResult.Value()));
    }
}