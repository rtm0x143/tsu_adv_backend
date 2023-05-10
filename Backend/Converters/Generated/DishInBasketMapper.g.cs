using System;
using System.Linq.Expressions;
using Backend.Features.Order.Domain;

namespace Backend.Converters
{
    public static partial class DishInBasketMapper
    {
        public static DishInOrderDto AdaptToDishInOrderDto(this DishInBasket p1)
        {
            return p1 == null ? default(DishInOrderDto) : new DishInOrderDto(p1.Dish, p1.Count);
        }
        public static DishInOrderDto AdaptTo(this DishInBasket p2, DishInOrderDto p3)
        {
            return p2 == null ? default(DishInOrderDto) : new DishInOrderDto(p2.Dish, p2.Count);
        }
        public static Expression<Func<DishInBasket, DishInOrderDto>> ProjectToDishInOrderDto => p4 => new DishInOrderDto(p4.Dish, p4.Count);
    }
}