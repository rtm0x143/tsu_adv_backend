using Common.Domain.Exceptions;
using Common.Domain.ValueTypes;
using Microsoft.AspNetCore.SignalR;
using Notifications.Converters;
using Notifications.Hubs;
using Notifications.Infra;
using OneOf;

namespace Notifications.Features.Notifications.Commands;

public class CommitNotification : ICommitNotification
{
    private readonly NotificationsDbContext _context;
    private readonly IHubContext<NotificationsHub, INotificationsClient> _hubContext;

    public CommitNotification(NotificationsDbContext context,
        IHubContext<NotificationsHub, INotificationsClient> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task<OneOf<EmptyResult, Exception>> Execute(CommitNotificationCommand command)
    {
        if (command.Notification.Id != default
            && await _context.Notifications.FindAsync(command.Notification.Id) != null)
            return new ConflictException("Already exists");

        _context.Notifications.Add(command.Notification);
        await _context.SaveChangesAsync();

        await _hubContext.Clients.ClientsByTopic(command.Notification.Topic)
            .ReceiveNotification(command.Notification.AdaptToExtensible())
            .ConfigureAwait(false);

        return default;
    }
}