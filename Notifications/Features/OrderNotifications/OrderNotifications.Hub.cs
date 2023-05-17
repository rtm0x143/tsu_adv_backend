using Common.App.Dtos;
using Common.App.Utils;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Notifications.Domain.Services;
using Notifications.Domain.ValueTypes;

// ReSharper disable once CheckNamespace
namespace Notifications.Hubs;

public partial class NotificationsHub
{
    [Authorize(Roles = nameof(CommonRoles.Customer))]
    public Task SubscribeToOrderTopic(string orderNumber)
    {
        var invariantString = OrderNumberFormatter.ToInvariantString(orderNumber);
        if (!invariantString.Succeeded()) throw invariantString.Error();

        var topicResult =
            NotificationTopic.Construct(OrderNotificationCreator.OrderTopicName, invariantString.Value());
        if (!topicResult.Succeeded()) throw topicResult.Error();

        return Groups.AddToGroupAsync(Context.ConnectionId, topicResult.Value().ToString());
    }
}