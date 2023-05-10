using Auth.Infra.Data.Entities;
using Auth.Infra.Data.IdentityServices;
using Auth.Converters;
using Common.App.Exceptions;
using Common.Domain.ValueTypes;
using OneOf;

namespace Auth.Features.User.Commands;

public class ChangeProfile : IChangeProfile
{
    private readonly AuthUserManager _userManager;
    public ChangeProfile(AuthUserManager userManager) => _userManager = userManager;

    public async Task<OneOf<EmptyResult, Exception>> Execute(ChangeProfileCommand command)
    {
        if (await _userManager.FindByIdAsync(command.UserId.ToString()) is not AppUser user)
            return new KeyNotFoundException(nameof(command.UserId));

        command.ProfileDto.AdaptTo(user);
        
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) return UnsuitableDataException.FromIdentityResult(result);
        return new EmptyResult();
    }
}