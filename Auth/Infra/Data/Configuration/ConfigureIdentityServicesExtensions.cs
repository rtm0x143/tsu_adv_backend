using System.Security.Claims;
using Auth.Infra.Data.Entities;
using Auth.Infra.Data.IdentityServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Auth.Infra.Data.Configuration;

public static class ConfigureIdentityServicesExtensions
{
    public const string AdditionalUserNameCharacters = " ";

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
                setup.User.AllowedUserNameCharacters += AdditionalUserNameCharacters;
            })
            .AddUserManager<AuthUserManager>()
            .AddRoles<RoleEntity>()
            .AddSignInManager()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();
    }
}