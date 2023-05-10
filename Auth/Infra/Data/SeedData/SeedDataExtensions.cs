using System.Security.Claims;
using Auth.Infra.Data.Entities;
using Auth.Infra.Data.IdentityServices;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Identity;

namespace Auth.Infra.Data.SeedData;

public static class SeedDataExtensions
{
    public record SeedUser(AppUser User, IEnumerable<CommonRoles> Roles, string? Password);

    public record SeedRole(RoleEntity Role, IEnumerable<Claim> Claims);

    public static async Task SeedDevelopmentData(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            var users = new SeedUser[]
            {
                new(new Admin { Email = "rtm0x143@mail.com", UserName = "rtm0x143" },
                    new[] { CommonRoles.Admin },
                    "Admin_323"),
                new(new Customer { Email = "customer1@example.com", UserName = "customer1" },
                    new[] { CommonRoles.Customer },
                    "String_0"),
                new(new Manager { Email = "manager@example.com", UserName = "Manager" },
                    new[] { CommonRoles.Manager },
                    "String_0"),
            };

            var userManager = scope.ServiceProvider.GetRequiredService<AuthUserManager>();
            foreach (var user in users)
            {
                var result = await userManager.CreateWithRolesAsync(user.User, user.Password, user.Roles.ToArray());
                if (result.Succeeded) logger.LogInformation($"User(UserName = {user.User.UserName}) created");
            }

            var roles = new SeedRole[]
            {
                new(new(CommonRoles.Admin), new Claim[]
                {
                    /*----- Grant -----*/
                    new(CommonClaimTypes.Grant, Enum.GetName(CommonRoles.Manager)!),
                    new(CommonClaimTypes.Grant, Enum.GetName(CommonRoles.Cook)!),
                    new(CommonClaimTypes.Grant, Enum.GetName(CommonRoles.Courier)!),
                    new(CommonClaimTypes.Grant, Enum.GetName(CommonRoles.RestaurantOwner)!),
                    new(CommonClaimTypes.Grant, Enum.GetName(CommonRoles.RestaurantAdmin)!),
                    /*----- PersonalData -----*/
                    new(CommonClaimTypes.PersonalData, Enum.GetName(CommonActionType.Change)!),
                    new(CommonClaimTypes.PersonalData, Enum.GetName(CommonActionType.Read)!)
                }),
                new(new(CommonRoles.RestaurantOwner), new Claim[]
                    { new(CommonClaimTypes.Grant, Enum.GetName(CommonRoles.RestaurantAdmin)!), }),
                new(new(CommonRoles.RestaurantAdmin), new Claim[]
                {
                    /*----- Grant -----*/
                    new(CommonClaimTypes.Grant, Enum.GetName(CommonRoles.Manager)!),
                    new(CommonClaimTypes.Grant, Enum.GetName(CommonRoles.Cook)!),
                    new(CommonClaimTypes.Grant, Enum.GetName(CommonRoles.Courier)!),
                }),
                new(new(CommonRoles.Cook), new Claim[]
                    { new(CommonClaimTypes.Manage, Enum.GetName(CommonManageTargets.Kitchen)!) }),
                new(new(CommonRoles.Courier), new Claim[]
                    { new(CommonClaimTypes.Manage, Enum.GetName(CommonManageTargets.Delivery)!) }),
            };

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role.Role);
                foreach (var claim in role.Claims) await roleManager.AddClaimAsync(role.Role, claim);
            }
        }
        catch (Exception e)
        {
            logger.LogDebug(e, "Exception while seeding database");
        }
    }
}