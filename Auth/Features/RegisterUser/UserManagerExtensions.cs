using Common.App.Dtos.Results;
using Common.App.Exceptions;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace Auth.Features.RegisterUser;

public static class UserManagerExtensions
{
    public static Task<OneOf<IdResult, UnsuitableDataException>> CreateAsyncChecked<TUser>(
        this UserManager<TUser> manager, TUser newUser, string password)
        where TUser : IdentityUser<Guid>
    {
        return manager.CreateAsync(newUser, password)
            .ContinueWith<OneOf<IdResult, UnsuitableDataException>>((t, user) =>
            {
                if (t.Result.Succeeded) return new IdResult(((TUser)user!).Id);
                return UnsuitableDataException.FromIdentityResult(t.Result);
            }, newUser);
    }
}