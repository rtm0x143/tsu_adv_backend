using Auth.Features.User.Commands;
using Auth.Infra.Data.Entities;

namespace Auth.Mappers.Generated
{
    public static partial class RegisterUserDtoMapper
    {
        public static AppUser AdaptToAppUser(this RegisterUserDto p1)
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
        public static AppUser AdaptTo(this RegisterUserDto p2, AppUser p3)
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
    }
}