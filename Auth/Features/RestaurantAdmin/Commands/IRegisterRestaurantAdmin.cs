using Auth.Features.Common;
using Common.App.Attributes;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Auth.Features.RestaurantAdmin.Commands;

public record RestaurantAdminRegistrationDto : RestaurantUserRegistrationDto;

public sealed record RegisterRestaurantAdminCommand(RestaurantAdminRegistrationDto RestaurantAdminDto) 
    : IRequestWithException<IdResult>;

[RequestHandlerInterface]
public interface IRegisterRestaurantAdmin : IRequestHandlerWithException<RegisterRestaurantAdminCommand, IdResult>
{
}