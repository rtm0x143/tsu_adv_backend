using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notifications.Domain.ValueTypes;

namespace Notifications.Infra.Configuration;

public class NotificationTopicConverter : ValueConverter<NotificationTopic, string>
{
    public NotificationTopicConverter()
        : base(topic => topic.ToString(),
            @string => NotificationTopic.FromString(@string)
                .Match(topic => topic, exception => new NotificationTopic()))
    {
    }
}