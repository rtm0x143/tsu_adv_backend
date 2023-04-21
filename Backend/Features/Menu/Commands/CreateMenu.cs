using Backend.Features.Menu.Commands;
using Backend.Features.Menu.Common;
using Backend.Infra.Data;
using Common.App.Exceptions;
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
        [HttpPost("{restaurantId}/menu")]
        public async Task<ActionResult> CreateMenu(Guid restaurantId, MenuCreationDto menu,
            [FromServices] ICreateMenu createMenu)
        {
            if (await AuthorizationServiceExtensions.AuthorizeAsync(AuthService, User,
                    ManageMenuInRestaurantPolicy.Create(restaurantId)) is { Succeeded: false })
                return Forbid();

            var result = await createMenu.Execute(new(restaurantId, menu));
            return result.IsT0 ? Ok() : ExceptionsDescriber.Describe(result.Value);
        }
    }
}

namespace Backend.Features.Menu.Commands
{
    public class CreateMenu : ICreateMenu
    {
        private readonly BackendDbContext _context;
        public CreateMenu(BackendDbContext context) => _context = context;

        public async Task<OneOf<EmptyResult, Exception>> Execute(CreateMenuCommand command)
        {
            if (await _context.Restaurants.FindAsync(command.RestaurantId) == null)
                return new KeyNotFoundException(nameof(command.RestaurantId));

            var menu = new Infra.Data.Entities.Menu
            {
                Name = command.Menu.Name,
                RestaurantId = command.RestaurantId
            };

            if (await _context.Menus.ContainsAsync(menu))
                return new CollisionException("Such menu in restaurant already exist");

            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
            return new EmptyResult();
        }
    }
}