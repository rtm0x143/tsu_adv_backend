using System.ComponentModel.DataAnnotations;
using Auth.Infra.Data.Entities;

namespace Auth.Features.Common;

public record UserRegistrationDto
{
    public required string Fullname { get; set; }
    [EmailAddress] public required string Email { get; set; }
    public DateOnly? BirthDate { get; set; }
    public Gender Gender { get; set; }
    [Phone] public required string PhoneNumber { get; set; }
    /// <summary>
    /// If null, represents user who can't sign in
    /// </summary>
    public string? Password { get; set; }
}
