using Auth.Features.Common;
using Common.App.Attributes;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Auth.Features.Manager.Commands;

public record ManagerRegistrationDto : RestaurantUserRegistrationDto;

public sealed record RegisterManagerCommand(ManagerRegistrationDto ManagerDto) : IRequestWithException<IdResult>;

[RequestHandlerInterface]
public interface IRegisterManager : IRequestHandlerWithException<RegisterManagerCommand, IdResult>
{
}