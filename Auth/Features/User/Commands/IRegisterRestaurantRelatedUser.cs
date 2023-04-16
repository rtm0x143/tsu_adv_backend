using Auth.Infra.Data.Entities;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.Models.Results;
using Common.App.RequestHandlers;
using Common.Infra.Auth;

namespace Auth.Features.User.Commands;

/// <summary>
/// Command to register new user who has relation to Restaurant 
/// </summary>
/// <param name="UserEntity">Concrete user entity</param>
/// <param name="Password">If null, registers user without password</param>
/// <param name="Role">Role of <paramref name="UserEntity"/>></param>
/// <param name="RestaurantId">Id of related Restaurant</param>
/// <exception cref="KeyNotFoundException"></exception>
/// <exception cref="UnsuitableDataException"></exception>
public sealed record RegisterRestaurantRelatedUserCommand(AppUser UserEntity, string? Password, string Role,
    Guid RestaurantId) :
    IRequestWithException<IdResult>
{
    /// <summary>
    /// Constructor without <see cref="Password"/> which will be set to null
    /// </summary>
    public RegisterRestaurantRelatedUserCommand(AppUser userEntity, CommonRoles role, Guid restaurantId)
        : this(userEntity, null, Enum.GetName(role)!, restaurantId)
    {
    }

    public RegisterRestaurantRelatedUserCommand(AppUser userEntity, string? password, CommonRoles role,
        Guid restaurantId)
        : this(userEntity, password, Enum.GetName(role)!, restaurantId)
    {
    }
}

[RequestHandlerInterface]
public interface IRegisterRestaurantRelatedUser : IRequestHandlerWithException<RegisterRestaurantRelatedUserCommand, IdResult>
{
}