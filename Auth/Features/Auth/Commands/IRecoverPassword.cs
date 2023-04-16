using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Auth.Features.Auth.Commands;

/// <exception cref="KeyNotFoundException">User by <paramref name="UsersEmail"/> not found</exception>
/// <exception cref="InvalidCredentialException">When <paramref name="ChangePasswordToken"/> invalid</exception>
/// <exception cref="UnsuitableDataException">When <paramref name="NewPassword"/> is bad</exception>
public sealed record RecoverPasswordCommand([EmailAddress] string UsersEmail, string ChangePasswordToken, string NewPassword)
    : IRequestWithException<EmptyResult>
{
    [EmailAddress] public string UsersEmail { get; set; } = UsersEmail;
}

[RequestHandlerInterface]
public interface IRecoverPassword : IRequestHandlerWithException<RecoverPasswordCommand, EmptyResult>
{
}
