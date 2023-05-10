namespace Backend.Messaging.Events;

public record RestaurantCreatedEvent(Guid Id, string Name);

public record RestaurantDeletedEvent(Guid Id);