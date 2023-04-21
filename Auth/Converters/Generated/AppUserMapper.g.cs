using System;
using System.Linq.Expressions;
using Auth.Features.Common;
using Auth.Infra.Data.Entities;

namespace Auth.Converters
{
    public static partial class AppUserMapper
    {
        public static UserProfileDto AdaptToUserProfileDto(this AppUser p1)
        {
            return p1 == null ? null : new UserProfileDto()
            {
                Email = p1.Email,
                Fullname = p1.UserName,
                PhoneNumber = p1.PhoneNumber,
                Gender = p1.Gender,
                BirthDate = p1.BirthDate
            };
        }
        public static UserProfileDto AdaptTo(this AppUser p2, UserProfileDto p3)
        {
            if (p2 == null)
            {
                return null;
            }
            UserProfileDto result = p3 ?? new UserProfileDto();
            
            result.Email = p2.Email;
            result.Fullname = p2.UserName;
            result.PhoneNumber = p2.PhoneNumber;
            result.Gender = p2.Gender;
            result.BirthDate = p2.BirthDate;
            return result;
            
        }
        public static Expression<Func<AppUser, UserProfileDto>> ProjectToUserProfileDto => p4 => new UserProfileDto()
        {
            Email = p4.Email,
            Fullname = p4.UserName,
            PhoneNumber = p4.PhoneNumber,
            Gender = p4.Gender,
            BirthDate = p4.BirthDate
        };
    }
}