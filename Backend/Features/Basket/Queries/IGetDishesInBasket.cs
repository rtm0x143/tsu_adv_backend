using Backend.Common.Dtos;
using Common.App.Attributes;
using Common.App.RequestHandlers;

namespace Backend.Features.Basket.Queries;

public sealed record GetDishesInBasketQuery(Guid UserId) : IRequest<DishShortDto[]>;

[RequestHandlerInterface]
public interface IGetDishesInBasket : IRequestHandler<GetDishesInBasketQuery, DishShortDto[]>
{
}