using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AdminPanel.Models;

public class SignInViewModel
{
    [Required] [EmailAddress] public string Email { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
}