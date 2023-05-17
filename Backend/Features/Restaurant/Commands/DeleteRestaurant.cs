using Backend.Features.Restaurant.Commands;
using Backend.Infra.Data;
using Backend.Messaging.Messages.Events;
using Common.App.Utils;
using Common.Domain.ValueTypes;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace Backend.Controllers
{
    public partial class RestaurantController
    {
        /// <summary>
        /// Delete restaurant
        /// </summary>
        /// <response code="404"></response>
        /// <response code="401"></response>
        /// <response code="403">When user has no permissions to create restaurant</response>
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
        private readonly IMessageSession _messageSession;

        public DeleteRestaurant(BackendDbContext context, IMessageSession messageSession)
        {
            _context = context;
            _messageSession = messageSession;
        }

        public async Task<OneOf<EmptyResult, Exception>> Execute(DeleteRestaurantCommand command)
        {
            if (await _context.Restaurants
                    .FindAsync(command.RestaurantId) is not Infra.Data.Entities.Restaurant restaurant)
                return new KeyNotFoundException(nameof(command.RestaurantId));

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            await _messageSession.Publish(new RestaurantDeletedEvent(command.RestaurantId))
                .ConfigureAwait(false);
            return new EmptyResult();
        }
    }
}