using Backend.Common.Dtos;
using Backend.Features.Basket.Queries;
using Backend.Infra.Data;
using Common.Infra.Auth;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    public partial class BasketController
    {
        [Authorize(Roles = nameof(CommonRoles.Customer))]
        [HttpGet]
        public Task<ActionResult<DishShortDto[]>> Get([FromServices] IGetDishesInBasket getDishesInBasket)
        {
            if (Guid.TryParse(GetUserId(), out var userId))
                return Task.FromResult<ActionResult<DishShortDto[]>>(InvalidTokenPayload());

            return getDishesInBasket.Execute(new(userId))
                .ContinueWith<ActionResult<DishShortDto[]>>(t => Ok(t.Result));
        }
    }
}

namespace Backend.Features.Basket.Queries
{
    public class GetDishesInBasket : IGetDishesInBasket
    {
        private readonly BackendDbContext _context;
        public GetDishesInBasket(BackendDbContext context) => _context = context;

        public Task<DishShortDto[]> Execute(GetDishesInBasketQuery query) =>
            _context.DishesInCart.Where(cart => cart.UserId == query.UserId)
                .ProjectToType<DishShortDto>()
                .ToArrayAsync();
    }
}