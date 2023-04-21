using Backend.Common.Dtos;
using Backend.Infra.Data.Entities;

namespace Backend.Converters
{
    public static partial class DishShortDtoMapper
    {
        public static Dish AdaptTo(this DishShortDto p1, Dish p2)
        {
            if (p1 == null)
            {
                return null;
            }
            Dish result = p2 ?? new Dish();
            
            result.Id = p1.Id;
            result.Name = p1.Name;
            result.Photo = p1.Photo;
            result.Price = p1.Price;
            result.Category = p1.Category;
            result.IsVegetarian = p1.IsVegetarian;
            return result;
            
        }
    }
}