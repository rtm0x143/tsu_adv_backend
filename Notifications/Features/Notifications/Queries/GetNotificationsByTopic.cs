using System.ComponentModel.DataAnnotations;
using Common.App.Utils;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notifications.Converters;
using Notifications.Domain.ValueTypes;
using Notifications.Features.Notifications.Common;
using Notifications.Features.Notifications.Queries;
using Notifications.Infra;

namespace Notifications.Controllers
{
    public partial class NotificationsController
    {
        [Authorize(Roles = nameof(CommonRoles.Admin))]
        [HttpGet]
        public Task<ActionResult<ExtensibleNotification[]>> Get([FromQuery, Required] string topic,
            [FromServices] IGetNotificationsByTopic getNotificationsByTopic)
        {
            var fromString = NotificationTopic.FromString(topic);
            if (!fromString.Succeeded())
                return Task.FromResult<ActionResult<ExtensibleNotification[]>>(
                    ExceptionsDescriber.Describe(fromString.Error()));

            return ExecuteRequest(getNotificationsByTopic, new(fromString.Value()));
        }
    }
}

namespace Notifications.Features.Notifications.Queries
{
    public class GetNotificationsByTopic : IGetNotificationsByTopic
    {
        private readonly NotificationsDbContext _context;
        public GetNotificationsByTopic(NotificationsDbContext context) => _context = context;

        public Task<ExtensibleNotification[]> Execute(GetNotificationsByTopicQuery query)
        {
            return _context.Notifications.Where(n => n.Topic == query.Topic)
                .Select(NotificationMapper.ProjectToExtensible)
                .AsNoTracking()
                .ToArrayAsync();
        }
    }
}