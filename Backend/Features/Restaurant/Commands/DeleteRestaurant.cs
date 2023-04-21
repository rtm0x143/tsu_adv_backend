using Backend.Features.Restaurant.Commands;
using Backend.Infra.Data;
using Common.App.Models.Results;
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
        [HttpDelete("{id}")]
        public Task<ActionResult<IdResult>> Delete(Guid id, [FromServices] IDeleteRestaurant deleteRestaurant)
        {
            return deleteRestaurant.Execute(new(id))
                .ContinueWith<ActionResult<IdResult>>(t => t.Result.Succeeded()
                    ? Ok()
                    : ExceptionsDescriber.Describe(t.Result.Error()));
        }
    }
}

namespace Backend.Features.Restaurant.Commands
{
    public class DeleteRestaurant : IDeleteRestaurant
    {
        private readonly BackendDbContext _context;
        public DeleteRestaurant(BackendDbContext context) => _context = context;

        public async Task<OneOf<EmptyResult, Exception>> Execute(DeleteRestaurantCommand command)
        {
            if (await _context.Restaurants
                    .FindAsync(command.RestaurantId) is not Infra.Data.Entities.Restaurant restaurant)
                return new KeyNotFoundException(nameof(command.RestaurantId));

            _context.Restaurants.Remove(restaurant);
            // TODO : dispatch restaurant deleted event to MQ
            await _context.SaveChangesAsync();
            return new EmptyResult();
        }
    }
}