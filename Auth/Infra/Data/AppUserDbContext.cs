using Auth.Infra.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infra.Data;

public class AppUserDbContext : IdentityDbContext<
    AppUser,
    RoleEntity,
    Guid,
    IdentityUserClaim<Guid>,
    AppUserRole,
    IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>,
    IdentityUserToken<Guid>>
{
    public DbSet<Customer> Customers { get; set; } = default!;

    public AppUserDbContext(DbContextOptions options) : base(options)
    {
    }
}