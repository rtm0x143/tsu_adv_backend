using System;
using Auth.Features.RegisterUser;
using Auth.Infra.Data.Entities;

namespace Auth.Mappers.Generated
{
    public static partial class RegisterCustomerCommandMapper
    {
        private static Action<RegisterCustomerCommand, CustomerData> Action1;
        
        public static CustomerData AdaptToCustomerData(this RegisterCustomerCommand p1)
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
        public static CustomerData AdaptTo(this RegisterCustomerCommand p2, CustomerData p3)
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