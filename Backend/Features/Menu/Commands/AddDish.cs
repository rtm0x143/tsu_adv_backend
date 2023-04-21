using Backend.Features.Menu.Commands;
using Backend.Features.Menu.Common;
using Backend.Infra.Data;
using Backend.Infra.Data.Entities;
using Common.App.Exceptions;
using Common.App.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using EmptyResult = Common.App.Models.Results.EmptyResult;

namespace Backend.Controllers
{
    public partial class RestaurantController
    {
        [Authorize]
        [HttpPost("{restaurantId}/menu/{menuName}/dish/{dishId}")]
        public async Task<ActionResult> AddDishToMenu(Guid restaurantId, string menuName, Guid dishId,
            [FromServices] IAddDish addDish)
        {
            if (await AuthService
                    .AuthorizeAsync(User, ManageMenuInRestaurantPolicy.Create(restaurantId)) is { Succeeded: false })
                return Forbid();

            var result = await addDish.Execute(new(restaurantId, menuName, dishId));
            return result.Succeeded() ? Ok() : ExceptionsDescriber.Describe(result.Value);
        }
    }
}

namespace Backend.Features.Menu.Commands
{
    public class AddDish : IAddDish
    {
        private readonly BackendDbContext _context;
        public AddDish(BackendDbContext context) => _context = context;

        public async Task<OneOf<EmptyResult, Exception>> Execute(AddDishCommand command)
        {
            var menu = await _context.Menus
                .Include(m => m.Dishes)
                .FirstOrDefaultAsync(m => m.RestaurantId == command.RestaurantId && m.Name == command.MenuName);

            if (menu == null)
                return new KeyNotFoundException($"{nameof(command.RestaurantId)}+{nameof(command.MenuName)}");

            if (await _context.Dishes.FindAsync(command.DishId) is not Dish dish)
                return new KeyNotFoundException(nameof(command.DishId));

            if (menu.Dishes!.Contains(dish)) return new CollisionException("Menu Already contains such dish");

            menu.Dishes!.Add(dish);
            await _context.SaveChangesAsync();
            return new EmptyResult();
        }
    }
}