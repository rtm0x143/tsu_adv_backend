using System;
using System.Linq.Expressions;
using Auth.Features.Common;
using Auth.Infra.Data.Entities;

namespace Auth.Converters
{
    public static partial class UserProfileDtoMapper
    {
        public static AppUser AdaptToAppUser(this UserProfileDto p1)
        {
            return p1 == null ? null : new AppUser()
            {
                Gender = p1.Gender,
                BirthDate = p1.BirthDate,
                UserName = p1.Fullname,
                Email = p1.Email,
                PhoneNumber = p1.PhoneNumber
            };
        }
        public static AppUser AdaptTo(this UserProfileDto p2, AppUser p3)
        {
            if (p2 == null)
            {
                return null;
            }
            AppUser result = p3 ?? new AppUser();
            
            result.Gender = p2.Gender;
            result.BirthDate = p2.BirthDate;
            result.UserName = p2.Fullname;
            result.Email = p2.Email;
            result.PhoneNumber = p2.PhoneNumber;
            return result;
            
        }
        public static Expression<Func<UserProfileDto, AppUser>> ProjectToAppUser => p4 => new AppUser()
        {
            Gender = p4.Gender,
            BirthDate = p4.BirthDate,
            UserName = p4.Fullname,
            Email = p4.Email,
            PhoneNumber = p4.PhoneNumber
        };
    }
}