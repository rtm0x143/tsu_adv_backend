using System.ComponentModel.DataAnnotations;
using Auth.Infra.Data.Entities;

namespace Auth.Features.Common;

public record UserProfileDto
{
    [EmailAddress] public string Email { get; set; } = default!; // explicit prop for framework use
    public string Fullname { get; set; } = default!;
    [Phone] public string PhoneNumber { get; set; } = default!;
    public Gender Gender { get; set; }
    public DateOnly? BirthDate { get; set; }
}