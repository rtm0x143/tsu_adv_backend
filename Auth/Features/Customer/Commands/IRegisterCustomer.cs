using Auth.Features.Common;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.Models.Results;
using Common.App.RequestHandlers;
using OneOf;

namespace Auth.Features.Customer.Commands;

public record CustomerRegistrationDto : RestaurantUserRegistrationDto
{
    public required string Address { get; set; }
}

public sealed record RegisterCustomerCommand(CustomerRegistrationDto CustomerRegistrationDto) :
    IRequestWithException<IdResult, UnsuitableDataException>;

[RequestHandlerInterface]
public interface IRegisterCustomer : IRequestHandlerWithException<RegisterCustomerCommand,  IdResult, UnsuitableDataException>
{
}