using System;
using System.Linq.Expressions;
using Backend.Common.Dtos;
using Backend.Features.Dish.Queries;
using Backend.Infra.Data.Entities;

namespace Backend.Converters
{
    public static partial class DishMapper
    {
        public static DishShortDto AdaptToShortDto(this Dish p1)
        {
            return p1 == null ? null : new DishShortDto(p1.Id, p1.Name, p1.Photo, p1.Price, p1.Category, p1.IsVegetarian);
        }
        public static Expression<Func<Dish, DishShortDto>> ProjectToShortDto => p2 => new DishShortDto(p2.Id, p2.Name, p2.Photo, p2.Price, p2.Category, p2.IsVegetarian);
        public static DishDto AdaptToDto(this Dish p3)
        {
            return p3 == null ? null : new DishDto(p3.Id, p3.Name, p3.Price, p3.Photo, p3.Category, p3.IsVegetarian, p3.Description, p3.CachedRate == null ? default(RateDto) : new RateDto(p3.CachedRate.Score, p3.CachedRate.Count), p3.Restaurant == null ? null : new RestaurantDto(p3.Restaurant.Id, p3.Restaurant.Name));
        }
        public static Expression<Func<Dish, DishDto>> ProjectToDto => p4 => new DishDto(p4.Id, p4.Name, p4.Price, p4.Photo, p4.Category, p4.IsVegetarian, p4.Description, new RateDto(p4.CachedRate.Score, p4.CachedRate.Count), new RestaurantDto(p4.Restaurant.Id, p4.Restaurant.Name));
    }
}