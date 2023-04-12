using System.ComponentModel.DataAnnotations;
using Auth.Infra.Data.Entities;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.Models.Results;
using Common.App.UseCases;
using OneOf;

namespace Auth.Features.User.Commands;

public record RegisterUserDto
{
    public required string Fullname { get; set; }
    [EmailAddress] public required string Email { get; set; }
    public DateOnly? BirthDate { get; set; }
    public Gender Gender { get; set; }
    [Phone] public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
}

public sealed record RegisterUserCommand(RegisterUserDto UserDto) : IAsyncRequest<OneOf<IdResult, UnsuitableDataException>>;

[UseCaseInterface]
public interface IRegisterUser : IAsyncUseCase<RegisterUserCommand, OneOf<IdResult, UnsuitableDataException>>
{
}