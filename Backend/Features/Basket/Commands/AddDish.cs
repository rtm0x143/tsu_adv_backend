using Backend.Features.Basket.Commands;
using Backend.Infra.Data;
using Backend.Infra.Data.Entities;
using Common.Domain.Exceptions;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace Backend.Controllers
{
    public partial class BasketController
    {
        /// <summary>
        /// Add dish to basket
        /// </summary>
        /// <response code="409">When basket contain dishes from another restaurant</response>
        /// <response code="404">When dish not found</response>
        /// <response code="401"></response>
        /// <response code="403">When user isn't customer</response>
        [Authorize(Roles = nameof(CommonRoles.Customer))]
        [HttpPost("{dishId}")]
        public Task<ActionResult> AddDish([FromServices] IAddDish addDish, Guid dishId, [FromQuery] uint count = 1)
        {
            if (!Guid.TryParse(GetUserId(), out var userId)) return Task.FromResult(InvalidTokenPayload());

            return addDish.Execute(new(userId, dishId, count))
                .ContinueWith(t => t.Result.IsT0
                    ? Ok()
                    : ExceptionsDescriber.Describe(t.Result.Value));
        }
    }
}

namespace Backend.Features.Basket.Commands
{
    public class AddDish : IAddDish
    {
        private readonly BackendDbContext _dbContext;
        public AddDish(BackendDbContext dbContext) => _dbContext = dbContext;

        public async Task<OneOf<EmptyResult, Exception>> Execute(AddDishCommand command)
        {
            if (await _dbContext.DishesInBasket
                    .FirstOrDefaultAsync(basket => basket.UserId == command.UserId && basket.DishId == command.DishId)
                is DishInBasket inBasket)
            {
                inBasket.Count += 1;
                await _dbContext.SaveChangesAsync();
                return new EmptyResult();
            }

            if (await _dbContext.Dishes.FindAsync(command.DishId) is not Infra.Data.Entities.Dish dish)
                return new KeyNotFoundException(nameof(command.DishId));

            if (await _dbContext.DishesInBasket
                    .Where(basket => basket.UserId == command.UserId && basket.Dish.RestaurantId != dish.RestaurantId)
                    .Take(1)
                    .CountAsync() == 1)
                return new ActionFailedException("Basket can contain dishes from only one restaurant");

            await _dbContext.DishesInBasket.AddAsync(new() { Dish = dish, UserId = command.UserId, Count = 1 });

            await _dbContext.SaveChangesAsync();
            return new EmptyResult();
        }
    }
}