using Microsoft.AspNetCore.SignalR;
using Notifications.Features.Notifications.Common;

namespace Notifications.Hubs;

public interface INotificationsClient
{
    Task ReceiveNotification(ExtensibleNotification notification);
}

public partial class NotificationsHub : Hub<INotificationsClient>
{
}