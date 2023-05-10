using Backend.Features.Menu.Commands;
using Backend.Features.Menu.Common;
using Backend.Infra.Data;
using Common.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace Backend.Controllers
{
    public partial class RestaurantController
    {
        /// <summary>
        /// Create new menu in specified restaurant
        /// </summary>
        /// <response code="404"></response>
        /// <response code="401"></response>
        /// <response code="403">When user can't manage menu</response>
        // [Authorize]
        [HttpPost("{restaurantId}/menu")]
        public async Task<ActionResult> CreateMenu(Guid restaurantId, MenuCreationDto menu,
            [FromServices] ICreateMenu createMenu)
        {
            if (await AuthService.AuthorizeAsync(User, ManageMenuInRestaurantPolicy.Create(restaurantId))
                is { Succeeded: false })
                return Forbid();

            return await ExecuteRequest(createMenu, new(restaurantId, menu));
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