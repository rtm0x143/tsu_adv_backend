using Auth.Infra.Data;
using Auth.Infra.Data.Entities;
using Backend.Messaging.Events;
using Microsoft.EntityFrameworkCore;
using NServiceBus;

namespace Auth.Events;

public class RestaurantDeletedEventHandler : IHandleMessages<RestaurantDeletedEvent>
{
    private readonly AuthDbContext _context;
    public RestaurantDeletedEventHandler(AuthDbContext context) => _context = context;

    public async Task Handle(RestaurantDeletedEvent @event, IMessageHandlerContext context)
    {
        if (await _context.Restaurants.FirstOrDefaultAsync(rest => rest.Id == @event.Id,
                context.CancellationToken) is Restaurant restaurant)
        {
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync(context.CancellationToken);
        }
    }
}