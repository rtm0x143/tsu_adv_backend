using System;
using System.Linq.Expressions;
using Auth.Features.Customer.Queries;
using Auth.Infra.Data.Entities;

namespace Auth.Mappers.Generated
{
    public static partial class CustomerMapper
    {
        public static CustomerProfileDto AdaptToProfileDto(this Customer p1)
        {
            return p1 == null ? null : new CustomerProfileDto(null, p1.Email, p1.PhoneNumber, p1.Gender, p1.BirthDate, p1.Address);
        }
        public static CustomerProfileDto AdaptTo(this Customer p2, CustomerProfileDto p3)
        {
            return p2 == null ? null : new CustomerProfileDto(null, p2.Email, p2.PhoneNumber, p2.Gender, p2.BirthDate, p2.Address);
        }
        public static Expression<Func<Customer, CustomerProfileDto>> ProjectToProfileDto => p4 => new CustomerProfileDto(null, p4.Email, p4.PhoneNumber, p4.Gender, p4.BirthDate, p4.Address);
    }
}