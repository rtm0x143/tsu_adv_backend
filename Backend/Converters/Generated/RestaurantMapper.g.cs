using System;
using System.Linq.Expressions;
using Backend.Features.Restaurant.Common;
using Backend.Infra.Data.Entities;

namespace Backend.Converters
{
    public static partial class RestaurantMapper
    {
        public static RestaurantDto AdaptToDto(this Restaurant p1)
        {
            return p1 == null ? null : new RestaurantDto(p1.Id, p1.Name);
        }
        public static RestaurantDto AdaptTo(this Restaurant p2, RestaurantDto p3)
        {
            return p2 == null ? null : new RestaurantDto(p2.Id, p2.Name);
        }
        public static Expression<Func<Restaurant, RestaurantDto>> ProjectToDto => p4 => new RestaurantDto(p4.Id, p4.Name);
    }
}