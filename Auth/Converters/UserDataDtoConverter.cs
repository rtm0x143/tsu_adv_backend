using System.Linq.Expressions;
using Auth.Features.User.Common;
using Auth.Infra.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auth.Converters;

public class UserDataDtoConverter
{
    public static Expression<Func<AppUser, UserDataDto>> CreateProjection(
        IQueryable<IdentityUserClaim<Guid>> identityUserClaims)
    {
        return user => new UserDataDto(
            user.Id,
            user.Roles.Select(userRole => userRole.Role.Name)
                .OfType<string>()
                .ToArray(),
            identityUserClaims.Where(claim => claim.UserId == user.Id)
                .Select(claim => new KeyValuePair<string, string?>(claim.ClaimType!, claim.ClaimValue))
                .ToArray())
        {
            Email = user.Email!,
            Fullname = user.UserName!,
            BirthDate = user.BirthDate,
            PhoneNumber = user.PhoneNumber!
        };
    }
}