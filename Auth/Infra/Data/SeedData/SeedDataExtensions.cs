using Auth.Infra.Data.Entities;
using Auth.Infra.Data.IdentityServices;
using Common.Infra.Auth;

namespace Auth.Infra.Data.SeedData;

public static class SeedDataExtensions
{
    public static async Task SeedDevelopmentData(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            var userManager = scope.ServiceProvider.GetRequiredService<AuthUserManager>();
            var admin = new Admin
            {
                Email = "rtm0x143@mail.com",
                UserName = "rtm0x143",
            };

            var result = await userManager.CreateWithRolesAsync(admin, "Admin_323", CommonRoles.Admin);
            if (result.Succeeded) logger.LogInformation($"Created admin user({admin.Id})");
        }
        catch (Exception e)
        {
            logger.LogDebug(e, "Exception while seeding database");
        }
    }
}