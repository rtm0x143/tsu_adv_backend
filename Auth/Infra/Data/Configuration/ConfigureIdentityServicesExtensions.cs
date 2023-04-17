using Auth.Infra.Data.Entities;
using Auth.Infra.Data.IdentityServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Auth.Infra.Data.Configuration;

public static class ConfigureIdentityServicesExtensions
{
    /// <summary>
    /// Configures <see cref="Microsoft.AspNetCore.Identity"/>, also applies <see cref="ConfigureDbContextExtensions.AddAuthDbContext"/>
    /// </summary>
    /// <returns><see cref="IdentityBuilder"/> for chaining</returns>
    public static IdentityBuilder AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddAuthDbContext(configuration)
            .AddIdentityCore<AppUser>(setup =>
            {
                setup.User.RequireUniqueEmail = true;
            })
            .AddUserManager<AuthUserManager>()
            .AddRoles<RoleEntity>()
            .AddSignInManager()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();
    }
}