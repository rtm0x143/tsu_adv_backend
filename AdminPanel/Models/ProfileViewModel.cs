using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models;

public class ProfileViewModel
{
    [EmailAddress] public string Email { get; set; } = default!;
    public string Fullname { get; set; } = default!;
    [Phone] public string PhoneNumber { get; set; } = default!;
    public string? Gender { get; set; }
    public DateOnly? BirthDate { get; set; }
}