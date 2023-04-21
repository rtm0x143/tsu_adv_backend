using Backend.Features.Basket;
using Backend.Features.Basket.Commands;
using Backend.Infra.Data;
using Backend.Infra.Data.Entities;
using Common.App.Exceptions;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using EmptyResult = Common.App.Models.Results.EmptyResult;

namespace Backend.Controllers
{
    public partial class BasketController
    {
        /// <summary>
        /// Add dish to basket
        /// </summary>
        /// <responce code="409">When basket contain dishes from another restaurant</responce>
        [Authorize(Roles = nameof(CommonRoles.Customer))]
        [HttpPost("{dishId}")]
        public Task<ActionResult> AddDish(Guid dishId, [FromServices] IAddDish addDish)
        {
            if (Guid.TryParse(GetUserId(), out var userId)) return Task.FromResult(InvalidTokenPayload()); 

            return addDish.Execute(new(default, dishId))
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
            if (await _dbContext.DishesInCart
                    .FirstOrDefaultAsync(cart => cart.UserId == command.UserId && cart.DishId == command.DishId)
                is DishInCart inCart)
            {
                inCart.Count += 1;
                await _dbContext.SaveChangesAsync();
                return new EmptyResult();
            }

            if (await _dbContext.Dishes.FindAsync(command.DishId) is not Dish dish)
                return new KeyNotFoundException(nameof(command.DishId));

            if (await _dbContext.DishesInCart
                    .Where(cart => cart.UserId == command.UserId && cart.Dish.RestaurantId != dish.RestaurantId)
                    .Take(1)
                    .CountAsync() == 1)
                return new ActionFailedException("Basket can contain dishes from only one restaurant");

            await _dbContext.DishesInCart.AddAsync(new() { Dish = dish, UserId = command.UserId });

            await _dbContext.SaveChangesAsync();
            return new EmptyResult();
        }
    }
}