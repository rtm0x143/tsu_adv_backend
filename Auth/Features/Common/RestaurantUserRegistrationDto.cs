namespace Auth.Features.Common;

public record RestaurantUserRegistrationDto : UserRegistrationDto
{
     public Guid RestaurantId { get; set; }
}