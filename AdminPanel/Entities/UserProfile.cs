using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Entities;

public record UserProfile([param: EmailAddress] string Email,
    string Fullname,
    [param: Phone] string PhoneNumber,
    string Gender,
    DateOnly? BirthDate);