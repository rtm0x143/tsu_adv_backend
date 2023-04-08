using Auth.Infra.Data.Entities;
using Auth.Infra.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Auth.Infra.Data.Configuration;

public static class ConfigureIdentityServicesExtensions
{
    /// <summary>
    /// Configures <see cref="Microsoft.AspNetCore.Identity"/>, also applies <see cref="ConfigureDbContextExtensions.AddAppUserDbContext"/>
    /// </summary>
    /// <returns><see cref="IdentityBuilder"/> for chaining</returns>
    public static IdentityBuilder AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        var builder = services.AddAppUserDbContext(configuration)
            .AddIdentity<AppUser, RoleEntity>()
            .AddEntityFrameworkStores<AuthDbContext>();
        
        return builder;
    }
}