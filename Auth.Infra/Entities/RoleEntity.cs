using System.ComponentModel.DataAnnotations.Schema;
using Auth.Api.Data;
using Microsoft.AspNetCore.Identity;

namespace Auth.Infra.Entities;

public class RoleEntity : IdentityRole<Guid>
{
    /// <summary> Preset widely used role names </summary>
    public enum Names
    {
        // User,
        Customer,
        Courier,
        Manager,
        Cook
    }

    public ICollection<AppUserRole>? AssociatedUsers { get; set; }

    public RoleEntity() { }
    public RoleEntity(Names role) : base(Enum.GetName(role)!) { }
}

public class AppUserRole : IdentityUserRole<Guid>
{
    [ForeignKey(nameof(UserId))]
    public AppUser User { get; set; }
    [ForeignKey(nameof(RoleId))]
    public RoleEntity Role { get; set; }
}