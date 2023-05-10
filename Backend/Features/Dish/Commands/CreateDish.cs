using Backend.Common.Dtos;
using Backend.Features.Dish.Commands;
using Backend.Features.Dish.Domain.Services;
using Backend.Features.Dish.Infra;
using Backend.Features.Menu.Common;
using Common.App.Attributes;
using Common.App.Utils;
using Common.Domain.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Backend.Controllers
{
    public partial class DishController
    {
        /// <summary>Create new dish</summary>
        /// <response code="404"></response>
        /// <response code="401"></response>
        /// <response code="403">When user can't manage menu</response>
        [VersioningApiRoute("restaurant/{restaurantId}/dish", OmitController = true)]
        [HttpPost]
        public async Task<ActionResult<IdResult>> Create(Guid restaurantId, DishCreationDto dish,
            [FromServices] ICreateDish createDish)
        {
            if (await AuthService
                    .AuthorizeAsync(User, ManageMenuInRestaurantPolicy.Create(restaurantId))
                is { Succeeded: false })
                return Forbid();

            return await ExecuteRequest(createDish, new(dish, restaurantId));
        }
    }
}

namespace Backend.Features.Dish.Commands
{
    public class CreateDish : ICreateDish
    {
        private readonly DishDbContext _context;
        public CreateDish(DishDbContext context) => _context = context;

        public async Task<OneOf<IdResult, Exception>> Execute(CreateDishCommand command)
        {
            if (await _context.Restaurants.FindAsync(command.RestaurantId) is not Domain.Restaurant restaurant)
                return new KeyNotFoundException(nameof(command.RestaurantId));

            var createResult = new DishCreator().CreateNew(
                command.Dish.Name, command.Dish.Price, command.Dish.Category,
                command.Dish.IsVegetarian, restaurant, command.Dish.Photo, command.Dish.Description);
            if (!createResult.Succeeded()) return createResult.Error();

            var entityEntry = _context.Dishes.Add(createResult.Value());

            await _context.SaveChangesAsync();
            return new IdResult(entityEntry.Entity.Id);
        }
    }
}