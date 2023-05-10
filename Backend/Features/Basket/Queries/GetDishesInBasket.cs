using Backend.Features.Basket.Queries;
using Backend.Infra.Data;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    public partial class BasketController
    {
        public record BasketDto(DishInBasketDto[] Dishes, decimal TotalPrice);

        /// <summary>
        /// Get all dishes in user's basket
        /// </summary>
        /// <param name="getDishesInBasket"></param>
        /// <response code="401"></response>
        /// <response code="403">When user isn't customer</response>
        [Authorize(Roles = nameof(CommonRoles.Customer))]
        [HttpGet]
        public Task<ActionResult<BasketDto>> Get([FromServices] IGetDishesInBasket getDishesInBasket)
        {
            if (!Guid.TryParse(GetUserId(), out var userId))
                return Task.FromResult<ActionResult<BasketDto>>(InvalidTokenPayload());

            return getDishesInBasket.Execute(new(userId))
                .ContinueWith<ActionResult<BasketDto>>(t => Ok(
                    new BasketDto(Dishes: t.Result,
                        TotalPrice: t.Result.Aggregate(0m, (sum, dto) => sum + dto.Price * dto.Count))));
        }
    }
}

namespace Backend.Features.Basket.Queries
{
    public class GetDishesInBasket : IGetDishesInBasket
    {
        private readonly BackendDbContext _context;
        public GetDishesInBasket(BackendDbContext context) => _context = context;

        public Task<DishInBasketDto[]> Execute(GetDishesInBasketQuery query) =>
            _context.DishesInBasket.Where(basket => basket.UserId == query.UserId)
                .Select(DishInBasketMapper.ProjectToDto)
                .AsNoTracking()
                .ToArrayAsync();
    }
}