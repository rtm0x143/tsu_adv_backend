using System;
using System.Linq.Expressions;
using Backend.Common.Dtos;
using Backend.Infra.Data.Entities;

namespace Backend.Converters
{
    public static partial class DishMapper
    {
        public static DishShortDto AdaptToShortDto(this Dish p1)
        {
            return p1 == null ? null : new DishShortDto(p1.Id, p1.Name, p1.Photo, p1.Price, p1.Category, p1.IsVegetarian);
        }
        public static DishShortDto AdaptTo(this Dish p2, DishShortDto p3)
        {
            return p2 == null ? null : new DishShortDto(p2.Id, p2.Name, p2.Photo, p2.Price, p2.Category, p2.IsVegetarian);
        }
        public static Expression<Func<Dish, DishShortDto>> ProjectToShortDto => p4 => new DishShortDto(p4.Id, p4.Name, p4.Photo, p4.Price, p4.Category, p4.IsVegetarian);
    }
}