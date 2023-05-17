using Microsoft.AspNetCore.SignalR;
using Notifications.Domain.ValueTypes;

namespace Notifications.Hubs;

public static class HubClientsExtensions
{
    public static TClient ClientsByTopic<TClient>(this IHubClients<TClient> clients, in NotificationTopic topic)
        where TClient : class
    {
        if (topic.IsDirect(out var id)) return clients.User(id);
        if (topic.IsBroadcast()) return clients.All;
        return clients.Group(topic.ToString());
    }
}