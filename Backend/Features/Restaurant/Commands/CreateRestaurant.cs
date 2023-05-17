using Backend.Features.Restaurant.Commands;
using Backend.Features.Restaurant.Common;
using Backend.Infra.Data;
using Backend.Messaging.Messages.Events;
using Common.App.Utils;
using Common.Domain.Exceptions;
using Common.Domain.ValueTypes;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using OneOf;

namespace Backend.Controllers
{
    public partial class RestaurantController
    {
        /// <summary>
        /// Create new restaurant 
        /// </summary>
        /// <response code="409">When restaurant's name already taken</response>
        /// <response code="401"></response>
        /// <response code="403">When user has no permissions to create restaurant</response>
        // [Authorize(Roles = nameof(CommonRoles.Admin))]
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
        private readonly IMessageSession _messageSession;

        public CreateRestaurant(BackendDbContext context, IMessageSession messageSession)
        {
            _context = context;
            _messageSession = messageSession;
        }

        public async Task<OneOf<IdResult, CollisionException>> Execute(CreateRestaurantCommand command)
        {
            // if (await _context.Restaurants.FirstOrDefaultAsync(r => r.Name == command.Restaurant.Name) != null)
            //     return new CollisionException($"Restaurant with name '{command.Restaurant.Name}' already exist");

            var entry = _context.Restaurants.Add(new() { Name = command.Restaurant.Name });
            // await _context.SaveChangesAsync();

            await _messageSession.Publish(new RestaurantCreatedEvent(entry.Entity.Id, entry.Entity.Name));

            return new IdResult(entry.Entity.Id);
        }
    }
}