using System;
using System.Linq.Expressions;
using Auth.Features.Customer.Common;
using Auth.Infra.Data.Entities;

namespace Auth.Mappers.Generated
{
    public static partial class CustomerProfileDtoMapper
    {
        public static Customer AdaptToCustomer(this CustomerProfileDto p1)
        {
            return p1 == null ? null : new Customer()
            {
                Address = p1.Address,
                Gender = p1.Gender,
                BirthDate = p1.BirthDate,
                Email = p1.Email,
                PhoneNumber = p1.PhoneNumber
            };
        }
        public static Customer AdaptTo(this CustomerProfileDto p2, Customer p3)
        {
            if (p2 == null)
            {
                return null;
            }
            Customer result = p3 ?? new Customer();
            
            result.Address = p2.Address;
            result.Gender = p2.Gender;
            result.BirthDate = p2.BirthDate;
            result.Email = p2.Email;
            result.PhoneNumber = p2.PhoneNumber;
            return result;
            
        }
        public static Expression<Func<CustomerProfileDto, Customer>> ProjectToCustomer => p4 => new Customer()
        {
            Address = p4.Address,
            Gender = p4.Gender,
            BirthDate = p4.BirthDate,
            Email = p4.Email,
            PhoneNumber = p4.PhoneNumber
        };
    }
}