using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;
using Notifications.Domain;

namespace Notifications.Features.Notifications.Commands;

public sealed record CommitNotificationCommand(Notification Notification) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface ICommitNotification : IRequestHandlerWithException<CommitNotificationCommand, EmptyResult>
{
}