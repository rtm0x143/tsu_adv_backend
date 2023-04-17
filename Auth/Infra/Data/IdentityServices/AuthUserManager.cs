using Auth.Infra.Data.Entities;
using Common.Infra.Auth;
using Common.Infra.Dal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Auth.Infra.Data.IdentityServices;

public class AuthUserManager : UserManager<AppUser>
{
    private readonly AuthDbContext _dbContext;

    public AuthUserManager(AuthDbContext dbContext, IUserStore<AppUser> store,
        IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<AppUser> passwordHasher,
        IEnumerable<IUserValidator<AppUser>> userValidators,
        IEnumerable<IPasswordValidator<AppUser>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<AppUser>> logger) : base(store,
        optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _dbContext = dbContext;
    }

    public async Task<IdentityResult> CreateWithRolesAsync(AppUser user, string? password, IEnumerable<string> roles)
    {
        await using var tr = await _dbContext.Database.BeginNestingTransaction();

        var result = await (password != null ? CreateAsync(user, password) : CreateAsync(user));
        if (!result.Succeeded) return result;

        result = await AddToRolesAsync(user, roles);
        await (result.Succeeded ? tr.CommitAsync() : tr.RollbackAsync());
        return result;
    }

    public Task<IdentityResult> CreateWithRolesAsync(AppUser user, string? password, params CommonRoles[] roles) =>
        CreateWithRolesAsync(user, password, roles.Select(Enum.GetName).Where(val => val != null)!);


    public async Task<IdentityResult> AddToRestaurantAsync(AppUser user, Restaurant restaurant)
    {
        var collision = await _dbContext.RestaurantAssociationRoleClaims
            .FirstOrDefaultAsync(claim => claim.UserId == user.Id
                                          && claim.RestaurantId == restaurant.Id);
        if (collision != null)
            return IdentityResult.Failed(ErrorDescriber.UserAlreadyInRestaurant(user.Id, restaurant.Id));

        await _dbContext.RestaurantAssociationRoleClaims.AddAsync(new()
        {
            RestaurantId = restaurant.Id,
            UserId = user.Id
        });
        await _dbContext.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> SetPasswordAsync(AppUser user, string password)
    {
        if (Store is not IUserPasswordStore<AppUser> passwordStore)
            throw new NotSupportedException($"{nameof(Store)} doesn't implement {nameof(IUserPasswordStore<AppUser>)}");
        
        var result = await ValidatePasswordAsync(user, password);
        if (!result.Succeeded) return result;

        var passwordHash = PasswordHasher.HashPassword(user, password);
        await passwordStore.SetPasswordHashAsync(user, passwordHash, default);
        
        return IdentityResult.Success;
    }
}