using System.ComponentModel.DataAnnotations;
using Auth.Infra.Data.Entities;

namespace Auth.Features.Common;

public record UserProfileDto(
    string Fullname,
    [EmailAddress] string Email,
    string PhoneNumber,
    Gender Gender,
    DateOnly? BirthDate)
{
    // ReSharper disable once RedundantExplicitPositionalPropertyDeclaration
    [EmailAddress] public string Email { get; init; } = Email;
}