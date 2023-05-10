using System.Linq.Expressions;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Basket.Queries;

public static class DishInBasketMapper
{
    public static Expression<Func<Entities.DishInBasket, DishInBasketDto>> ProjectToDto => d => new(
        d.Dish.Id,
        d.Dish.Name,
        d.Dish.Photo,
        d.Dish.Price,
        d.Dish.Category,
        d.Dish.IsVegetarian,
        d.Count);
}