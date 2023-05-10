using Backend.Common.Dtos;

namespace Backend.Converters
{
    public static partial class OrderNumberMapper
    {
        public static ulong AdaptToulong(this OrderNumber p1)
        {
            ulong result = p1.Numeric;
            return result;
            
        }
    }
}