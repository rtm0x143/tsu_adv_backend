using Auth.Features.Common;
using Common.App.Attributes;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Auth.Features.Courier.Commands;

public record CourierRegistrationDto : UserRegistrationDto;

public sealed record RegisterCourierCommand(CourierRegistrationDto CourierDto) : IRequestWithException<IdResult>;

[RequestHandlerInterface]
public interface IRegisterCourier : IRequestHandlerWithException<RegisterCourierCommand, IdResult>
{
}