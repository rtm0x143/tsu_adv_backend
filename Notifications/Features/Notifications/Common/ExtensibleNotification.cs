using System.Text.Json.Serialization;

namespace Notifications.Features.Notifications.Common;

public class ExtensibleNotification
{
    public required DateTime NotifyTime { get; init; }
    public required string Topic { get; init; }
    public required string Title { get; init; }
    [JsonExtensionData] public Dictionary<string, object>? Extensions { get; set; }
}