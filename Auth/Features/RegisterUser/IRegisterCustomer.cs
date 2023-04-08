using Common.App.Attributes;
using Common.App.Dtos.Results;
using Common.App.Exceptions;
using Common.App.UseCases;
using OneOf;

namespace Auth.Features.RegisterUser;

[UseCaseInterface]
public interface IRegisterCustomer : IAsyncUseCase<RegisterCustomerCommand, OneOf<IdResult, UnsuitableDataException>>
{
}