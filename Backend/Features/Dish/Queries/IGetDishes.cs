using System.Text.Json.Serialization;
using Backend.Common.Dtos;
using Backend.Features.Dish.Domain.ValueTypes;
using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.RequestHandlers;
using Common.Infra.Dal;

namespace Backend.Features.Dish.Queries;

public record struct RateDto(float Score, ulong Count);

public record DishDto(
        Guid Id,
        string Name,
        decimal Price,
        string? Photo,
        DishCategory Category,
        bool IsVegetarian,
        string? Description,
        RateDto Rate,
        RestaurantDto Restaurant)
    : DishShortDto(Id, Name, Photo, Price, Category, IsVegetarian);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DishSortFactor
{
    Unspecified,
    Name,
    Price,
    Rating,
}

public record MenuDto(Guid RestaurantId, string Name);

/// <exception cref="UnsuitableDataException">
/// When <paramref name="RestaurantId"/> and <paramref name="Menu"/> both specified
/// but <paramref name="RestaurantId"/> != <paramref name="Menu"/>.RestaurantId
/// </exception>
public sealed record GetDishesQuery(
        PaginationInfo<Guid> Pagination,
        Guid RestaurantId = default,
        MenuDto? Menu = default,
        DishCategory[]? Categories = null,
        bool OnlyVegetarian = false,
        DishSortFactor SortFactor = DishSortFactor.Unspecified,
        SortType SortType = SortType.Dec)
    : IRequestWithException<DishDto[], UnsuitableDataException>;

[RequestHandlerInterface]
public interface IGetDishes : IRequestHandlerWithException<GetDishesQuery, DishDto[], UnsuitableDataException>
{
}