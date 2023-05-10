using Backend.Features.Dish.Commands;
using Backend.Features.Dish.Infra;
using Backend.Features.Menu.Common;
using Common.App.Attributes;
using Common.App.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace Backend.Controllers
{
    public partial class DishController
    {
        /// <summary>Delete dish</summary>
        /// <response code="404"></response>
        /// <response code="401"></response>
        /// <response code="403">When user can't manage menu</response>
        [VersioningApiRoute("restaurant/{restaurantId}/dish/{id}", OmitController = true)]
        [HttpDelete]
        public async Task<ActionResult> Delete(Guid restaurantId, Guid id, [FromServices] IDeleteDish deleteDish)
        {
            if (await AuthService.AuthorizeAsync(User, ManageMenuInRestaurantPolicy.Create(restaurantId))
                is { Succeeded: false })
                return Forbid();

            return await ExecuteRequest(deleteDish, new(restaurantId, id));
        }
    }
}

namespace Backend.Features.Dish.Commands
{
    public class DeleteDish : IDeleteDish
    {
        private readonly DishDbContext _context;
        public DeleteDish(DishDbContext context) => _context = context;

        public async Task<OneOf<EmptyResult, Exception>> Execute(DeleteDishCommand command)
        {
            if (await _context.Dishes.FirstOrDefaultAsync(d =>
                    d.Id == command.DishId && d.RestaurantId == command.RestaurantId) is not Domain.Dish dish)
                return new KeyNotFoundException($"{nameof(command.RestaurantId)}+{nameof(command.DishId)}");

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return new EmptyResult();
        }
    }
}