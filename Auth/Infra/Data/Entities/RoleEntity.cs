using Common.Infra.Auth;
using Microsoft.AspNetCore.Identity;

namespace Auth.Infra.Data.Entities;

public class RoleEntity : IdentityRole<Guid>
{
    public ICollection<AppUserRole>? AssociatedUsers { get; set; }

    public RoleEntity() { }
    public RoleEntity(CommonRoles role) : base(Enum.GetName(role)!) { }
}

public class AppUserRole : IdentityUserRole<Guid>
{
    public AppUser User { get; set; } = default!;
    public RoleEntity Role { get; set; } = default!;
}