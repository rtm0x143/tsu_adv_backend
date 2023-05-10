using System;
using Backend.Common.Dtos;
using Backend.Features.Dish.Domain;

namespace Backend.Converters
{
    public static partial class DishCreationDtoMapper
    {
        public static Dish AdaptTo(this DishCreationDto p1, Dish p2)
        {
            if (p1 == null)
            {
                return null;
            }
            Dish result = p2 ?? new Dish();
            
            result.Photo = p1.Photo == null ? null : (Uri)Convert.ChangeType((object)p1.Photo, typeof(Uri));
            result.Description = p1.Description;
            result.Category = p1.Category;
            result.IsVegetarian = p1.IsVegetarian;
            return result;
            
        }
    }
}