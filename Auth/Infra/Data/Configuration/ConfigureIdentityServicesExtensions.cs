using Auth.Infra.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auth.Infra.Data.Configuration;

public static class ConfigureIdentityServicesExtensions
{
    /// <summary>
    /// Configures <see cref="Microsoft.AspNetCore.Identity"/>, also applies <see cref="ConfigureDbContextExtensions.AddAppUserDbContext"/>
    /// </summary>
    /// <returns><see cref="IdentityBuilder"/> for chaining</returns>
    public static IdentityBuilder AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddAppUserDbContext(configuration)
            .AddIdentityCore<AppUser>(setup =>
            {
                setup.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
    }
}