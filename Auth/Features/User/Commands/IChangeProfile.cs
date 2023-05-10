using Auth.Features.Common;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Auth.Features.User.Commands;

/// <exception cref="KeyNotFoundException">Customer with <paramref name="UserId"/> not found</exception>
/// <exception cref="UnsuitableDataException">When <paramref name="ProfileDto"/> invalid</exception>
public sealed record ChangeProfileCommand(Guid UserId, UserProfileDto ProfileDto) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface IChangeProfile : IRequestHandlerWithException<ChangeProfileCommand, EmptyResult>
{
}