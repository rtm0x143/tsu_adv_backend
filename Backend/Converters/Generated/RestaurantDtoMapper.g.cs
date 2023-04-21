using System;
using System.Linq.Expressions;
using Backend.Features.Restaurant.Common;
using Backend.Infra.Data.Entities;

namespace Backend.Converters
{
    public static partial class RestaurantDtoMapper
    {
        public static Restaurant AdaptToRestaurant(this RestaurantDto p1)
        {
            return p1 == null ? null : new Restaurant()
            {
                Id = p1.Id,
                Name = p1.Name
            };
        }
        public static Restaurant AdaptTo(this RestaurantDto p2, Restaurant p3)
        {
            if (p2 == null)
            {
                return null;
            }
            Restaurant result = p3 ?? new Restaurant();
            
            result.Id = p2.Id;
            result.Name = p2.Name;
            return result;
            
        }
        public static Expression<Func<RestaurantDto, Restaurant>> ProjectToRestaurant => p4 => new Restaurant()
        {
            Id = p4.Id,
            Name = p4.Name
        };
    }
}