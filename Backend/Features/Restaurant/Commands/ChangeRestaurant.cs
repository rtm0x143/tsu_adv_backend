using Backend.Features.Restaurant.Commands;
using Backend.Features.Restaurant.Common;
using Backend.Infra.Data;
using Common.App.Utils;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using EmptyResult = Common.App.Models.Results.EmptyResult;


namespace Backend.Controllers
{
    public partial class RestaurantController
    {
        [Authorize(Roles = nameof(CommonRoles.Admin))]
        [HttpPut("{id}")]
        public Task<ActionResult> Update(Guid id, RestaurantCreationDto restaurant,
            [FromServices] IChangeRestaurant changeRestaurant)
        {
            return changeRestaurant.Execute(new(id, restaurant))
                .ContinueWith<ActionResult>(t => t.Result.Succeeded()
                    ? Ok()
                    : Conflict(t.Result.Error()));
        }
    }
}

namespace Backend.Features.Restaurant.Commands
{
    public class ChangeRestaurant : IChangeRestaurant
    {
        private readonly BackendDbContext _context;
        public ChangeRestaurant(BackendDbContext context) => _context = context;

        public async Task<OneOf<EmptyResult, Exception>> Execute(ChangeRestaurantCommand command)
        {
            if (await _context.Restaurants
                    .FindAsync(command.RestaurantId) is not Infra.Data.Entities.Restaurant restaurant)
                return new KeyNotFoundException(nameof(command.RestaurantId));

            restaurant.Name = command.Restaurant.Name;
            await _context.SaveChangesAsync();
            return new EmptyResult();
        }
    }
}