using Backend.Features.Basket.Commands;
using Backend.Infra.Data;
using Backend.Infra.Data.Entities;
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
        /// Decreases amount of dishes in basket 
        /// </summary>
        /// <param name="removeDish"></param>
        /// <param name="dishId">Dish to find in basket</param>
        /// <param name="truncate">If true removes all dishes in basket</param>
        /// <param name="count">Number by which to reduce the amount</param>
        /// <response code="404">When dish wasn't found in user's basket</response>
        /// <response code="401"></response>
        /// <response code="403">When user isn't customer</response>
        [Authorize(Roles = nameof(CommonRoles.Customer))]
        [HttpDelete("{dishId}")]
        public Task<ActionResult> RemoveDish([FromServices] IRemoveDish removeDish, Guid dishId,
            [FromQuery] bool truncate = false, [FromQuery] uint count = 1)
        {
            if (!Guid.TryParse(GetUserId(), out var userId)) return Task.FromResult(InvalidTokenPayload());
            return removeDish.Execute(truncate 
                    ? RemoveDishCommand.RemoveAll(userId, dishId) 
                    : new(userId, dishId, count))
                .ContinueWith<ActionResult>(t => t.Result.IsT0 ? Ok() : NotFound());
        }
    }
}

namespace Backend.Features.Basket.Commands
{
    public class RemoveDish : IRemoveDish
    {
        private readonly BackendDbContext _context;
        public RemoveDish(BackendDbContext context) => _context = context;

        public async Task<OneOf<EmptyResult, KeyNotFoundException>> Execute(RemoveDishCommand command)
        {
            if (await _context.DishesInBasket
                    .FirstOrDefaultAsync(basket => basket.DishId == command.DishId && basket.UserId == command.UserId)
                is not DishInBasket inBasket)
                return new KeyNotFoundException($"{nameof(command.DishId)}+{nameof(command.UserId)}");

            if (inBasket.Count > command.Count) inBasket.Count -= command.Count;
            else _context.DishesInBasket.Remove(inBasket);

            await _context.SaveChangesAsync();
            return new EmptyResult();
        }
    }
}