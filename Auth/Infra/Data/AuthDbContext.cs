using Auth.Infra.Data.Configuration;
using Auth.Infra.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Auth.Infra.Data;

public class AuthDbContext : IdentityDbContext<
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
    public DbSet<Courier> Couriers { get; set; } = default!;
    public DbSet<Admin> Admins { get; set; } = default!;
    public DbSet<RestaurantOwner> RestaurantOwners { get; set; } = default!;
    public DbSet<RestaurantAdmin> RestaurantAdmins { get; set; } = default!;
    public DbSet<Manager> Managers { get; set; } = default!;
    public DbSet<Cook> Cooks { get; set; } = default!;
    
    public DbSet<RestaurantAssociationUserClaim> RestaurantAssociationRoleClaims { get; set; } = default!;
    public DbSet<Restaurant> Restaurants { get; set; } = default!;
    
    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; } = default!;

    public AuthDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>().UseTphMappingStrategy();
        
        builder.Entity<AppUserRole>()
            .HasOne<RoleEntity>(e => e.Role)
            .WithMany(e => e.AssociatedUsers)
            .HasForeignKey(e => e.RoleId);

        builder.Entity<AppUserRole>()
            .HasOne<AppUser>(e => e.User)
            .WithMany(e => e.Roles)
            .HasForeignKey(e => e.UserId);

        builder.Entity<RestaurantAssociationUserClaim>()
            .HasOne<Restaurant>()
            .WithMany()
            .HasForeignKey(c => c.RestaurantId);
        
        var configuration = this.GetService<IConfiguration>();
        builder.SeedRolesData(configuration)
            .SeedIdentityRoleClaimData(configuration);
    }
}