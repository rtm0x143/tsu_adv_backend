namespace Backend.Features.Restaurant.Common;

public record RestaurantCreationDto(string Name);

public record RestaurantDto(Guid Id, string Name);