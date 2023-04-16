using Auth.Infra.Data;
using Auth.Infra.Data.Entities;
using Auth.Infra.Data.IdentityServices;
using Common.App.Exceptions;
using Common.App.Models.Results;
using Common.Infra.Auth;
using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Auth.Features.User.Commands;

public class RegisterRestaurantRelatedUser : IRegisterRestaurantRelatedUser
{
    private readonly AuthUserManager _userManager;
    private readonly AuthDbContext _dbContext;

    public RegisterRestaurantRelatedUser(AuthUserManager userManager, AuthDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<OneOf<IdResult, Exception>> Execute(RegisterRestaurantRelatedUserCommand command)
    {
        if (await _dbContext.Restaurants.FindAsync(command.RestaurantId) is not Restaurant restaurant)
            return new KeyNotFoundException(nameof(command.RestaurantId));

        await using var trans = await _dbContext.Database.BeginNestingTransaction();

        var result = await _userManager
            .CreateWithRolesAsync(command.UserEntity, command.Password, new[] { command.Role });
        if (!result.Succeeded) return UnsuitableDataException.FromIdentityResult(result);

        result = await _userManager.AddToRestaurantAsync(command.UserEntity, restaurant);
        if (!result.Succeeded)
        {
            await trans.RollbackAsync();
            return UnsuitableDataException.FromIdentityResult(result);
        }

        await trans.CommitAsync();
        return new IdResult(command.UserEntity.Id);
    }
}