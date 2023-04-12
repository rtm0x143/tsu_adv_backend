using Auth.Features.User.Commands;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.Models.Results;
using Common.App.UseCases;
using OneOf;

namespace Auth.Features.Customer.Commands;

public record RegisterCustomerDto : RegisterUserDto
{
    public required string Address { get; set; }
}

public sealed record RegisterCustomerCommand(RegisterCustomerDto CustomerDto) :
    IAsyncRequest<OneOf<IdResult, UnsuitableDataException>>;

[UseCaseInterface]
public interface IRegisterCustomer : IAsyncUseCase<RegisterCustomerCommand, OneOf<IdResult, UnsuitableDataException>>
{
}