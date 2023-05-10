using Backend.Converters;
using Backend.Infra.Data;
using Common.App.Exceptions;
using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Dish.Queries;

public class GetDishes : IGetDishes
{
    private readonly BackendDbContext _context;
    public GetDishes(BackendDbContext context) => _context = context;

    private UnsuitableDataException AmbiguousRestaurantFilterException => new("Ambiguous restaurant filter")
    {
        Problems =
        {
            {
                nameof(GetDishesQuery.RestaurantId),
                $"Value mot match corresponding value in \"{nameof(GetDishesQuery.Menu)}\""
            },
            {
                $"{nameof(GetDishesQuery.Menu)}.{nameof(GetDishesQuery.Menu.RestaurantId)}",
                $"Value mot match corresponding value in \"{nameof(GetDishesQuery.RestaurantId)}\""
            }
        }
    };

    public Task<OneOf<DishDto[], UnsuitableDataException>> Execute(GetDishesQuery query)
    {
        if (query.RestaurantId != default
            && query.Menu != default
            && query.RestaurantId != query.Menu.RestaurantId)
            return Task.FromResult<OneOf<DishDto[], UnsuitableDataException>>(AmbiguousRestaurantFilterException);

        IQueryable<Entities.Dish> queryable;
        if (query.Menu != default)
            queryable = _context.Menus.Where(m => m.Name == query.Menu.Name
                                                  && m.RestaurantId == query.Menu.RestaurantId)
                .SelectMany(m => m.Dishes!);
        else
            queryable = query.RestaurantId == default
                ? _context.Dishes.AsQueryable()
                : _context.Dishes.Where(d => d.RestaurantId == query.RestaurantId);

        if (query.OnlyVegetarian) queryable = _context.Dishes.Where(d => d.IsVegetarian == query.OnlyVegetarian);
        if (!query.Categories.IsNullOrEmpty())
            queryable = queryable.Where(d => query.Categories!.Contains(d.Category));

        queryable = query.SortFactor switch
        {
            DishSortFactor.Name => queryable.OrderBy(d => d.Name, query.SortType),
            DishSortFactor.Price => queryable.OrderBy(d => d.Price, query.SortType),
            DishSortFactor.Rating => queryable.OrderBy(d => d.CachedRate.Score, query.SortType),
            _ => queryable
        };

        if (query.Pagination.AfterRecord != default)
            queryable = queryable.Where(d => d.Id > query.Pagination.AfterRecord);

        return queryable.Take((int)query.Pagination.PageSize)
            .Select(DishMapper.ProjectToDto)
            .ToArrayAsync()
            .ContinueWith(t => OneOf<DishDto[], UnsuitableDataException>.FromT0(t.Result));
    }
}