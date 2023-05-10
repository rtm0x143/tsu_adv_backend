using Backend.Common.Dtos;
using Backend.Features.Dish.Domain.ValueTypes;
using Common.App.Attributes;
using Common.App.RequestHandlers;

namespace Backend.Features.Basket.Queries;

public record DishInBasketDto(
        Guid Id, string Name, string? Photo, decimal Price, DishCategory Category, bool IsVegetarian,
        uint Count)
    : DishShortDto(Id, Name, Photo, Price, Category, IsVegetarian);

public sealed record GetDishesInBasketQuery(Guid UserId) : IRequest<DishInBasketDto[]>;

[RequestHandlerInterface]
public interface IGetDishesInBasket : IRequestHandler<GetDishesInBasketQuery, DishInBasketDto[]>
{
}