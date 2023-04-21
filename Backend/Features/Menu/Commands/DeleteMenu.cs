using Backend.Features.Menu.Commands;
using Backend.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using EmptyResult = Common.App.Models.Results.EmptyResult;

namespace Backend.Controllers
{
    public partial class RestaurantController
    {
        [HttpDelete("{restaurantId}/menu/{menuName}")]
        public Task<ActionResult> DeleteMenu(Guid restaurantId, string menuName, [FromServices] IDeleteMenu deleteMenu)
        {
            return deleteMenu.Execute(new(restaurantId, menuName))
                .ContinueWith<ActionResult>(t => t.Result.IsT0 ? Ok() : NotFound());
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