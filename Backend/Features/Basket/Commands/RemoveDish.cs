using Backend.Features.Basket.Commands;
using Backend.Infra.Data;
using Backend.Infra.Data.Entities;
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
        [Authorize(Roles = nameof(CommonRoles.Customer))]
        [HttpDelete("{dishId}")]
        public Task<ActionResult> RemoveDish([FromServices] IRemoveDish removeDish, Guid dishId,
            [FromQuery] bool truncate = false, [FromQuery] uint count = 1)
        {
            if (Guid.TryParse(GetUserId(), out var userId)) return Task.FromResult(InvalidTokenPayload());
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
            if (await _context.DishesInCart
                    .FirstOrDefaultAsync(cart => cart.DishId == command.DishId && cart.UserId == command.UserId)
                is not DishInCart inCart)
                return new KeyNotFoundException($"{nameof(command.DishId)}+{nameof(command.UserId)}");

            if (inCart.Count > command.Count) inCart.Count -= command.Count;
            else _context.DishesInCart.Remove(inCart);

            await _context.SaveChangesAsync();
            return new EmptyResult();
        }
    }
}