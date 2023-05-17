using Common.App.Dtos;

namespace Backend.Messaging.Messages.Events;

public record OrderStatusChangedEvent(OrderNumber OrderNumber, Guid UserId, string OrderStatus, string? Description);