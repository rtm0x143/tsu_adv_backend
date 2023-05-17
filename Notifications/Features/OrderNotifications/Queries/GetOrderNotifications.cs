using Common.App.Dtos;
using Common.App.Utils;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.Domain.Services;
using Notifications.Features.Notifications.Common;
using Notifications.Features.Notifications.Queries;
using Notifications.Features.OrderNotifications.Queries;
using OneOf;

namespace Notifications.Controllers
{
    public partial class NotificationsController
    {
        [Authorize(Roles = nameof(CommonRoles.Customer))]
        [HttpGet("order/{orderNumber}")]
        public Task<ActionResult<ExtensibleNotification[]>> GetOrderNotifications(
            OrderNumber orderNumber,
            [FromServices] IGetOrderNotifications getOrderNotifications)
            => ExecuteRequest(getOrderNotifications, new(orderNumber));
    }
}

namespace Notifications.Features.OrderNotifications.Queries
{
    public class GetOrderNotifications : IGetOrderNotifications
    {
        private readonly IGetNotificationsByTopic _getNotificationsByTopic;

        public GetOrderNotifications(IGetNotificationsByTopic getNotificationsByTopic) =>
            _getNotificationsByTopic = getNotificationsByTopic;

        public async Task<OneOf<ExtensibleNotification[], Exception>> Execute(GetOrderNotificationsQuery query)
        {
            var topicResult = new OrderNotificationCreator().CreateTopic(query.OrderNumber.String);
            if (!topicResult.Succeeded()) return topicResult.Error();

            return await _getNotificationsByTopic.Execute(new(topicResult.Value()));
        }
    }
}