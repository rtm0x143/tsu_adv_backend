using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Auth.Features.Auth.Commands;

/// <exception cref="KeyNotFoundException">User by <see cref="UsersEmail"/> not found</exception>
/// <exception cref="InvalidCredentialException">When <paramref name="ChangePasswordToken"/> invalid</exception>
/// <exception cref="UnsuitableDataException">When <paramref name="NewPassword"/> is bad</exception>
public sealed record RecoverPasswordCommand(string ChangePasswordToken, string NewPassword)
    : IRequestWithException<EmptyResult>
{
    [EmailAddress] public required string UsersEmail { get; set; } // explicit prop for framework use 
}

[RequestHandlerInterface]
public interface IRecoverPassword : IRequestHandlerWithException<RecoverPasswordCommand, EmptyResult>
{
}