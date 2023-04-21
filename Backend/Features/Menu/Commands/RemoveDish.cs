using Backend.Features.Menu.Common;
using Backend.Infra.Data;
using Backend.Infra.Data.Entities;
using Common.App.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using EmptyResult = Common.App.Models.Results.EmptyResult;
using IRemoveDish = Backend.Features.Menu.Commands.IRemoveDish;

namespace Backend.Controllers
{
    public partial class RestaurantController
    {
        [Authorize]
        [HttpDelete("{restaurantId}/menu/{menuName}/dish/{dishId}")]
        public async Task<ActionResult> RemoveDishFromMenu(Guid restaurantId, string menuName, Guid dishId,
            [FromServices] IRemoveDish removeDish)
        {
            if (await AuthService
                    .AuthorizeAsync(User, ManageMenuInRestaurantPolicy.Create(restaurantId)) is { Succeeded: false })
                return Forbid();

            var result = await removeDish.Execute(new(restaurantId, menuName, dishId));
            return result.Succeeded() ? Ok() : NotFound(result.AsT1.Message);
        }
    }
}

namespace Backend.Features.Menu.Commands
{
    public class RemoveDish : IRemoveDish
    {
        private readonly BackendDbContext _context;
        public RemoveDish(BackendDbContext context) => _context = context;

        public async Task<OneOf<EmptyResult, Exception>> Execute(RemoveDishCommand command)
        {
            if (await _context.Dishes.FindAsync(command.DishId) is not Dish dish)
                return new KeyNotFoundException(nameof(command.DishId));

            var menu = await _context.Menus
                .Include(m => m.Dishes)
                .FirstOrDefaultAsync(m => m.RestaurantId == command.RestaurantId
                                          && m.Name == command.MenuName
                                          && m.Dishes!.Contains(dish));

            if (menu == null)
            {
                return new KeyNotFoundException(
                    $"{nameof(command.RestaurantId)}+{nameof(command.MenuName)}+{nameof(dish.Name)}");
            }

            menu.Dishes!.Remove(dish);
            await _context.SaveChangesAsync();
            return new EmptyResult();
        }
    }
}