using Microsoft.AspNetCore.Identity;

namespace Auth.Infra.Data.IdentityServices;

internal static class IdentityErrorDescriberExtensions
{
    internal static IdentityError UserAlreadyInRestaurant(this IdentityErrorDescriber describer, Guid userId, Guid restaurantId) => new()
    {
        Code = nameof(UserAlreadyInRestaurant),
        Description = $"That user({userId}) associated with restaurant({restaurantId})"
    };
}