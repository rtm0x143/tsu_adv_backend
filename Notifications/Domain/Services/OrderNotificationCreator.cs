using Common.App.Utils;
using Notifications.Domain.ValueTypes;
using OneOf;

namespace Notifications.Domain.Services;

public class OrderNotificationCreator
{
    public const string OrderTopicName = "Order";

    public OneOf<NotificationTopic, ArgumentException> CreateTopic(string orderNumber)
        => NotificationTopic.Construct(OrderTopicName, orderNumber);

    public OneOf<Notification, ArgumentException> CreateNewStatusChanged(string newStatus, string orderNumber,
        string? description = null)
    {
        var topicResult = CreateTopic(orderNumber);
        if (!topicResult.Succeeded()) return topicResult.Error();

        var payload = new Dictionary<string, object>()
        {
            { nameof(newStatus), newStatus },
            { nameof(orderNumber), orderNumber }
        };
        if (description != null) payload.Add(nameof(description), description);

        return new Notification
        {
            Id = Guid.NewGuid(),
            NotifyTime = DateTime.UtcNow,
            Title = "Order status changed",
            Topic = topicResult.Value(),
            Payload = payload
        };
    }
}