using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Infra.Data.Entities;

public class CustomerData
{
    [Key] [ForeignKey(nameof(User))] public Guid UserId { get; set; }
    [Required] public AppUser? User { get; set; }
    public string? Address { get; set; }
}