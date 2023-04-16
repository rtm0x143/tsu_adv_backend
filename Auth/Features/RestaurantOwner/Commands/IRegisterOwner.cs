using Auth.Features.Common;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Auth.Features.RestaurantOwner.Commands;

public record RestaurantOwnerRegistrationDto : RestaurantUserRegistrationDto;

public sealed record RegisterOwnerCommand(RestaurantOwnerRegistrationDto OwnerDto) : IRequestWithException<IdResult>;

public interface IRegisterOwner : IRequestHandlerWithException<RegisterOwnerCommand, IdResult>
{
}