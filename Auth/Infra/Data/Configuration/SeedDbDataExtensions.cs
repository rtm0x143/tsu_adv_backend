using System.Text.Json;
using Auth.Infra.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infra.Data.Configuration;

/// <summary>
/// Deprecated
/// </summary>
public static class SeedDbDataExtensions
{
    public static ModelBuilder SeedRolesData(this ModelBuilder builder, IConfiguration configuration)
    {
        var roles = JsonSerializer.Deserialize<RoleEntity[]>(
            File.ReadAllText(Path.Join(configuration["SeedDataDir"], $"{nameof(RoleEntity)}.json")));

        if (!roles.IsNullOrEmpty())
            builder.Entity<RoleEntity>().HasData(roles!);

        return builder;
    }
    
    public static ModelBuilder SeedIdentityRoleClaimData(this ModelBuilder builder, IConfiguration configuration)
    {
        var roleClaims = JsonSerializer.Deserialize<IdentityRoleClaim<Guid>[]>(
            File.ReadAllText(Path.Join(configuration["SeedDataDir"], $"{nameof(IdentityRoleClaim<Guid>)}.json")));

        if (!roleClaims.IsNullOrEmpty())
            builder.Entity<IdentityRoleClaim<Guid>>().HasData(roleClaims!);
        
        return builder;
    }
}