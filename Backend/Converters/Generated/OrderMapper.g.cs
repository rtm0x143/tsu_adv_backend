using System;
using System.Linq;
using System.Linq.Expressions;
using Backend.Common.Dtos;
using Backend.Converters;
using Backend.Infra.Data.Entities;

namespace Backend.Converters
{
    public static partial class OrderMapper
    {
        public static OrderShortDto AdaptToShortDto(this Order p1)
        {
            return p1 == null ? null : new OrderShortDto(new OrderNumber(p1.Number, OrderNumberFormatter.Encode(p1.Number)), p1.Price, p1.Address, p1.Status, p1.CreatedTime, p1.DeliveryTime);
        }
        public static Expression<Func<Order, OrderShortDto>> ProjectToShortDto => p2 => new OrderShortDto(new OrderNumber(p2.Number, OrderNumberFormatter.Encode(p2.Number)), p2.Price, p2.Address, p2.Status, p2.CreatedTime, p2.DeliveryTime);
        public static OrderDto AdaptToDto(this Order p3)
        {
            return p3 == null ? null : new OrderDto(new OrderNumber(p3.Number, OrderNumberFormatter.Encode(p3.Number)), p3.Price, p3.Address, p3.Status, p3.CreatedTime, p3.DeliveryTime, funcMain1(p3.Dishes.Select<DishInOrder, DishShortDto>(funcMain2).ToArray<DishShortDto>()), p3.UserId, p3.Restaurant == null ? null : new RestaurantDto(p3.Restaurant.Id, p3.Restaurant.Name));
        }
        public static Expression<Func<Order, OrderDto>> ProjectToDto => p5 => new OrderDto(new OrderNumber(p5.Number, OrderNumberFormatter.Encode(p5.Number)), p5.Price, p5.Address, p5.Status, p5.CreatedTime, p5.DeliveryTime, p5.Dishes.Select<DishInOrder, DishShortDto>(dish => dish.Dish.AdaptToShortDto()).ToArray<DishShortDto>(), p5.UserId, new RestaurantDto(p5.Restaurant.Id, p5.Restaurant.Name));
        
        private static DishShortDto[] funcMain1(DishShortDto[] p4)
        {
            if (p4 == null)
            {
                return null;
            }
            DishShortDto[] result = new DishShortDto[p4.Length];
            
            int v = 0;
            
            int i = 0;
            int len = p4.Length;
            
            while (i < len)
            {
                DishShortDto item = p4[i];
                result[v++] = item == null ? null : new DishShortDto(item.Id, item.Name, item.Photo, item.Price, item.Category, item.IsVegetarian);
                i++;
            }
            return result;
            
        }
        
        private static DishShortDto funcMain2(DishInOrder dish)
        {
            return dish.Dish.AdaptToShortDto();
        }
    }
}