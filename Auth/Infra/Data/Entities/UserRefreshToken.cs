using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Infra.Data.Entities;

public class UserRefreshToken
{
    public Guid Id { get; set; }
    [ForeignKey(nameof(User))] public Guid UserId { get; set; }
    public AppUser User { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; } = false;
}