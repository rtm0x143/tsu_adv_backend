using Common.App.Attributes;
using Common.App.RequestHandlers;
using Notifications.Domain.ValueTypes;
using Notifications.Features.Notifications.Common;

namespace Notifications.Features.Notifications.Queries;

public sealed record GetNotificationsByTopicQuery(NotificationTopic Topic) : IRequest<ExtensibleNotification[]>;

[RequestHandlerInterface]
public interface IGetNotificationsByTopic : IRequestHandler<GetNotificationsByTopicQuery, ExtensibleNotification[]>
{
}