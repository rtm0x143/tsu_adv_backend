using Backend.Features.Menu.Commands;
using Backend.Features.Menu.Common;
using Backend.Infra.Data;
using Common.App.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace Backend.Controllers
{
    public partial class RestaurantController
    {
        /// <summary>Delete some menu in specified restaurant</summary>
        /// <response code="404"></response>
        /// <response code="401"></response>
        /// <response code="403">When user can't manage menu</response>
        [HttpDelete("{restaurantId}/menu/{menuName}")]
        public async Task<ActionResult> DeleteMenu(Guid restaurantId, string menuName,
            [FromServices] IDeleteMenu deleteMenu)
        {
            if (await AuthService.AuthorizeAsync(User, ManageMenuInRestaurantPolicy.Create(restaurantId))
                is { Succeeded: false })
                return Forbid();
            
            var result = await deleteMenu.Execute(new(restaurantId, menuName));
            return result.Succeeded() ? Ok() : NotFound(result.Error().Message);
        }
    }
}

namespace Backend.Features.Menu.Commands
{
    public class DeleteMenu : IDeleteMenu
    {
        private readonly BackendDbContext _context;
        public DeleteMenu(BackendDbContext context) => _context = context;

        public async Task<OneOf<EmptyResult, KeyNotFoundException>> Execute(DeleteMenuCommand command)
        {
            if (await _context.Menus.FirstOrDefaultAsync(m =>
                    m.RestaurantId == command.RestaurantId && m.Name == command.Name)
                is not Infra.Data.Entities.Menu menu)
                return new KeyNotFoundException($"{nameof(command.RestaurantId)}+{nameof(command.Name)}");

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();
            return new EmptyResult();
        }
    }
}