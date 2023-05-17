using Auth.Infra.Data;
using Auth.Infra.Data.Entities;
using Backend.Messaging.Messages.Events;
using Microsoft.EntityFrameworkCore;
using NServiceBus;

namespace Auth.EventHandlers;

public class RestaurantCreatedEventHandler : IHandleMessages<RestaurantCreatedEvent>
{
    private readonly AuthDbContext _context;
    public RestaurantCreatedEventHandler(AuthDbContext context) => _context = context;

    public async Task Handle(RestaurantCreatedEvent @event, IMessageHandlerContext context)
    {
        var restaurant = new Restaurant { Id = @event.Id };

        if (await _context.Restaurants.FirstOrDefaultAsync(rest => rest.Id == restaurant.Id,
                context.CancellationToken) != null)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync(context.CancellationToken);
        }
    }
}