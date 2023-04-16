using Auth.Features.Customer.Commands;
using Auth.Infra.Data.Entities;

namespace Auth.Mappers.Generated
{
    public static partial class CustomerRegistrationDtoMapper
    {
        public static Customer AdaptToCustomer(this CustomerRegistrationDto p1)
        {
            return p1 == null ? null : new Customer()
            {
                Address = p1.Address,
                Gender = p1.Gender,
                BirthDate = p1.BirthDate,
                UserName = p1.Fullname,
                Email = p1.Email,
                PhoneNumber = p1.PhoneNumber
            };
        }
        public static Customer AdaptTo(this CustomerRegistrationDto p2, Customer p3)
        {
            if (p2 == null)
            {
                return null;
            }
            Customer result = p3 ?? new Customer();
            
            result.Address = p2.Address;
            result.Gender = p2.Gender;
            result.BirthDate = p2.BirthDate;
            result.UserName = p2.Fullname;
            result.Email = p2.Email;
            result.PhoneNumber = p2.PhoneNumber;
            return result;
            
        }
    }
}