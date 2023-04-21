using Auth.Features.Common;
using Auth.Features.Customer.Commands;
using Auth.Infra.Data.Entities;

namespace Auth.Converters
{
    public static partial class UserRegistrationDtoMapper
    {
        public static AppUser AdaptToAppUser(this UserRegistrationDto p1)
        {
            CustomerRegistrationDto p2 = p1 as CustomerRegistrationDto;
            
            if (p2 != null)
            {
                return p2 == null ? null : new Customer()
                {
                    Address = p2.Address,
                    Gender = p2.Gender,
                    BirthDate = p2.BirthDate,
                    UserName = p2.Fullname,
                    Email = p2.Email,
                    PhoneNumber = p2.PhoneNumber
                };
            }
            
            if (p1 == null)
            {
                return null;
            }
            AppUser result = new AppUser();
            
            result.Gender = p1.Gender;
            result.BirthDate = p1.BirthDate;
            result.UserName = p1.Fullname;
            result.Email = p1.Email;
            result.PhoneNumber = p1.PhoneNumber;
            return result;
            
        }
        public static AppUser AdaptTo(this UserRegistrationDto p3, AppUser p4)
        {
            CustomerRegistrationDto p5 = p3 as CustomerRegistrationDto;
            Customer p6 = p4 as Customer;
            
            if (p5 != null && p6 != null)
            {
                return funcMain1(p5, p6);
            }
            
            if (p3 == null)
            {
                return null;
            }
            AppUser result = p4 ?? new AppUser();
            
            result.Gender = p3.Gender;
            result.BirthDate = p3.BirthDate;
            result.UserName = p3.Fullname;
            result.Email = p3.Email;
            result.PhoneNumber = p3.PhoneNumber;
            return result;
            
        }
        
        private static Customer funcMain1(CustomerRegistrationDto p7, Customer p8)
        {
            if (p7 == null)
            {
                return null;
            }
            Customer result = p8 ?? new Customer();
            
            result.Address = p7.Address;
            result.Gender = p7.Gender;
            result.BirthDate = p7.BirthDate;
            result.UserName = p7.Fullname;
            result.Email = p7.Email;
            result.PhoneNumber = p7.PhoneNumber;
            return result;
            
        }
    }
}