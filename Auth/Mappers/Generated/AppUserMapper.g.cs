using System;
using System.Linq.Expressions;
using Auth.Features.Common;
using Auth.Infra.Data.Entities;

namespace Auth.Mappers.Generated
{
    public static partial class AppUserMapper
    {
        public static UserProfileDto AdaptToUserProfileDto(this AppUser p1)
        {
            return p1 == null ? null : new UserProfileDto(p1.UserName, p1.Email, p1.PhoneNumber, p1.Gender, p1.BirthDate);
        }
        public static UserProfileDto AdaptTo(this AppUser p2, UserProfileDto p3)
        {
            return p2 == null ? null : new UserProfileDto(p2.UserName, p2.Email, p2.PhoneNumber, p2.Gender, p2.BirthDate);
        }
        public static Expression<Func<AppUser, UserProfileDto>> ProjectToUserProfileDto => p4 => new UserProfileDto(p4.UserName, p4.Email, p4.PhoneNumber, p4.Gender, p4.BirthDate);
    }
}