using Backend.Common.Dtos;
using Backend.Converters;
using Backend.Features.Dish.Commands;
using Backend.Features.Dish.Infra;
using Backend.Features.Menu.Common;
using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.App.Utils;
using Common.Domain.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace Backend.Controllers
{
    public partial class DishController
    {
        /// <summary>Change dish</summary>
        /// <response code="404"></response>
        /// <response code="401"></response>
        /// <response code="403">When user can't manage menu</response>
        [VersioningApiRoute("restaurant/{restaurantId}/dish/{id}", OmitController = true)]
        [HttpPut]
        public async Task<ActionResult<IdResult>> Update(Guid restaurantId, Guid id, DishCreationDto dish,
            [FromServices] IChangeDish changeDish)
        {
            if (await AuthService.AuthorizeAsync(User, ManageMenuInRestaurantPolicy.Create(restaurantId))
                is { Succeeded: false })
                return Forbid();

            return await ExecuteRequest(changeDish, new(id, dish, restaurantId));
        }
    }
}

namespace Backend.Features.Dish.Commands
{
    public class ChangeDish : IChangeDish
    {
        private readonly DishDbContext _context;
        public ChangeDish(DishDbContext context) => _context = context;

        public async Task<OneOf<EmptyResult, Exception>> Execute(ChangeDishCommand command)
        {
            if (await _context.Dishes.FirstOrDefaultAsync(
                    d => d.RestaurantId == command.RestaurantId && d.Id == command.DishId)
                is not Domain.Dish dish)
                return new KeyNotFoundException($"{nameof(command.RestaurantId)}+{nameof(command.DishId)}");

            if (dish.ChangeName(command.Dish.Name).Value is ArgumentException nameEx) return nameEx;
            if (dish.ChangePrice(command.Dish.Price).Value is ArgumentOutOfRangeException priceEx) return priceEx;
            command.Dish.AdaptTo(dish);

            await _context.SaveChangesAsync();
            return default;
        }
    }
}