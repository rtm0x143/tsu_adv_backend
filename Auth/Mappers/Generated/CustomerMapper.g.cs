using System;
using System.Linq.Expressions;
using Auth.Features.Customer.Common;
using Auth.Infra.Data.Entities;

namespace Auth.Mappers.Generated
{
    public static partial class CustomerMapper
    {
        public static CustomerProfileDto AdaptToProfileDto(this Customer p1)
        {
            return p1 == null ? null : new CustomerProfileDto()
            {
                Address = p1.Address,
                Email = p1.Email,
                PhoneNumber = p1.PhoneNumber,
                Gender = p1.Gender,
                BirthDate = p1.BirthDate
            };
        }
        public static CustomerProfileDto AdaptTo(this Customer p2, CustomerProfileDto p3)
        {
            if (p2 == null)
            {
                return null;
            }
            CustomerProfileDto result = p3 ?? new CustomerProfileDto();
            
            result.Address = p2.Address;
            result.Email = p2.Email;
            result.PhoneNumber = p2.PhoneNumber;
            result.Gender = p2.Gender;
            result.BirthDate = p2.BirthDate;
            return result;
            
        }
        public static Expression<Func<Customer, CustomerProfileDto>> ProjectToProfileDto => p4 => new CustomerProfileDto()
        {
            Address = p4.Address,
            Email = p4.Email,
            PhoneNumber = p4.PhoneNumber,
            Gender = p4.Gender,
            BirthDate = p4.BirthDate
        };
    }
}