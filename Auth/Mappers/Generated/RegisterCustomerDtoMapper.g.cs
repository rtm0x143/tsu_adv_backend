using System;
using Auth.Features.Customer.Commands;
using Auth.Infra.Data.Entities;

namespace Auth.Mappers.Generated
{
    public static partial class RegisterCustomerDtoMapper
    {
        private static Action<RegisterCustomerDto, CustomerData> Action1;
        
        public static CustomerData AdaptToCustomerData(this RegisterCustomerDto p1)
        {
            if (p1 == null)
            {
                return null;
            }
            CustomerData result = new CustomerData();
            Action1.Invoke(p1, result);
            
            result.Address = p1.Address;
            return result;
            
        }
        public static CustomerData AdaptTo(this RegisterCustomerDto p2, CustomerData p3)
        {
            if (p2 == null)
            {
                return null;
            }
            CustomerData result = p3 ?? new CustomerData();
            Action1.Invoke(p2, result);
            
            result.Address = p2.Address;
            return result;
            
        }
    }
}