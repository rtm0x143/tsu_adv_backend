using Auth.Features.Common;
using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Auth.Features.RestaurantAdmin.Commands;

public record RestaurantAdminRegistrationDto : RestaurantUserRegistrationDto;

public sealed record RegisterRestaurantAdminCommand(RestaurantAdminRegistrationDto RestaurantAdminDto) 
    : IRequestWithException<IdResult>;

[RequestHandlerInterface]
public interface IRegisterRestaurantAdmin : IRequestHandlerWithException<RegisterRestaurantAdminCommand, IdResult>
{
}