using System.Text.Json.Serialization;

namespace Notifications.Features.Notifications.Common;

public record ExtensibleNotification(DateTime NotifyTime, string Topic, string Title,
    [property: JsonExtensionData] Dictionary<string, object>? Extensions = null);