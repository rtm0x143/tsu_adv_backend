using Notifications.Domain.ValueTypes;

namespace Notifications.Domain;

public class Notification
{
    public required Guid Id { get; init; }
    public required DateTime NotifyTime { get; init; }
    public required NotificationTopic Topic { get; init; }
    public required string Title { get; set; }

    public required Dictionary<string, object> Payload { get; set; }
}