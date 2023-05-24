using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Entities;

public record User(
        [property: HiddenInput] Guid Id,
        [param: EmailAddress] string Email,
        string Fullname,
        [param: Phone] string PhoneNumber,
        string Gender,
        DateOnly? BirthDate,
        [property: HiddenInput] Guid? Restaurant,
        [property: HiddenInput] bool IsBanned,
        [property: HiddenInput] IEnumerable<string> Roles,
        [property: HiddenInput] KeyValuePair<string, string?>[] AllUserClaims)
    : UserProfile(Email, Fullname, PhoneNumber, Gender, BirthDate);