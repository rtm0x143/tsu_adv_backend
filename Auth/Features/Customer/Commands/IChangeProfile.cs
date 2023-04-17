using Auth.Features.Customer.Common;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Auth.Features.Customer.Commands;

/// <exception cref="KeyNotFoundException">Customer with <paramref name="CustomerId"/> not found</exception>
/// <exception cref="UnsuitableDataException">When <paramref name="ProfileDto"/> invalid</exception>
public sealed record ChangeProfileCommand(Guid CustomerId, CustomerProfileDto ProfileDto) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IChangeProfile : IRequestHandlerWithException<ChangeProfileCommand, EmptyResult>
{
}