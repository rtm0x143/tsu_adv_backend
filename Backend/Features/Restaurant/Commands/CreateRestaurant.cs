using Backend.Features.Restaurant.Commands;
using Backend.Features.Restaurant.Common;
using Backend.Infra.Data;
using Common.App.Exceptions;
using Common.App.Models.Results;
using Common.App.Utils;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Backend.Controllers
{
    public partial class RestaurantController
    {
        [Authorize(Roles = nameof(CommonRoles.Admin))]
        [HttpPost]
        public Task<ActionResult<IdResult>> Create(RestaurantCreationDto restaurant,
            [FromServices] ICreateRestaurant createRestaurant)
        {
            return createRestaurant.Execute(new(restaurant))
                .ContinueWith<ActionResult<IdResult>>(t => t.Result.Succeeded()
                    ? Ok(t.Result.Value())
                    : Conflict(t.Result.Error().Message));
        }
    }
}

namespace Backend.Features.Restaurant.Commands
{
    public class CreateRestaurant : ICreateRestaurant
    {
        private readonly BackendDbContext _context;
        public CreateRestaurant(BackendDbContext context) => _context = context;

        public async Task<OneOf<IdResult, CollisionException>> Execute(CreateRestaurantCommand command)
        {
            if (await _context.Restaurants.FirstOrDefaultAsync(r => r.Name == command.Restaurant.Name) != null)
                return new CollisionException($"Restaurant with name '{command.Restaurant.Name}' already exist");

            var entry = _context.Restaurants.Add(new() { Name = command.Restaurant.Name });
            // TODO : dispatch restaurant created event to MQ
            await _context.SaveChangesAsync();
            return new IdResult { Id = entry.Entity.Id };
        }
    }
}