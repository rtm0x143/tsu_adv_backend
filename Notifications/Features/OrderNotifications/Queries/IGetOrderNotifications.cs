using Common.App.Attributes;
using Common.App.Dtos;
using Common.App.RequestHandlers;
using Notifications.Features.Notifications.Common;

namespace Notifications.Features.OrderNotifications.Queries;

public sealed record GetOrderNotificationsQuery(OrderNumber OrderNumber)
    : IRequestWithException<ExtensibleNotification[]>;

[RequestHandlerInterface]
public interface IGetOrderNotifications
    : IRequestHandlerWithException<GetOrderNotificationsQuery, ExtensibleNotification[]>
{
}