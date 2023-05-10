using Auth.Features.Common;
using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Auth.Features.Cook.Commands;

public record CookRegistrationDto : RestaurantUserRegistrationDto;

public sealed record RegisterCookCommand(CookRegistrationDto CookDto) : IRequestWithException<IdResult>;

[RequestHandlerInterface]
public interface IRegisterCook : IRequestHandlerWithException<RegisterCookCommand, IdResult>
{
}