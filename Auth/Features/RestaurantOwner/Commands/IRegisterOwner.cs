using Auth.Features.Common;
using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Auth.Features.RestaurantOwner.Commands;

public record RestaurantOwnerRegistrationDto : RestaurantUserRegistrationDto;

public sealed record RegisterOwnerCommand(RestaurantOwnerRegistrationDto OwnerDto) : IRequestWithException<IdResult>;

[RequestHandlerInterface]
public interface IRegisterOwner : IRequestHandlerWithException<RegisterOwnerCommand, IdResult>
{
}