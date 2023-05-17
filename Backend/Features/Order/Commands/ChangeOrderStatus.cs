using Backend.Features.Order.Commands;
using Backend.Features.Order.Domain.ValueTypes;
using Backend.Features.Order.Infra;
using Backend.Messaging.Messages.Events;
using Common.App.Dtos;
using Common.App.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace Backend.Controllers
{
    public partial class OrderController
    {
        /// <summary>
        /// Change status of specified order
        /// </summary>
        /// <response code="404"></response>
        /// <response code="401"></response>
        /// <response code="403">When user can't interact with order in current state</response>
        [HttpPost("{number}/status/{status}")]
        public async Task<ActionResult> ChangeOrderStatus(
            [FromRoute] OrderNumber number,
            [FromRoute] OrderStatus status,
            [FromServices] IChangeOrderStatus changeOrderStatus)
        {
            var authorizationResult = await AuthService.AuthorizeAsync(User, status, ChangeOrderStatusPolicy.Instance);
            if (!authorizationResult.Succeeded) return Forbid();

            if (!Guid.TryParse(GetUserId(), out var userId)) return InvalidTokenPayload();

            var result = await changeOrderStatus.Execute(new(number.Numeric, userId, status));
            return result.Succeeded() ? Ok() : ExceptionsDescriber.Describe(result.Error());
        }
    }
}

namespace Backend.Features.Order.Commands
{
    public class ChangeOrderStatus : IChangeOrderStatus
    {
        private readonly OrderDbContext _context;
        private readonly IMessageSession _messageSession;

        public ChangeOrderStatus(OrderDbContext context, IMessageSession messageSession)
        {
            _context = context;
            _messageSession = messageSession;
        }

        public async Task<OneOf<EmptyResult, Exception>> Execute(ChangeStatusCommand command)
        {
            if (await _context.GetOrderByNumber(command.OrderNumber) is not Domain.Order order)
                return new KeyNotFoundException(nameof(command.OrderNumber));

            var result = order.ChangeStatus(command.Status, command.UserId);
            if (!result.Succeeded()) return result.Error();

            await _context.SaveChangesAsync();
            await _messageSession.Publish(new OrderStatusChangedEvent(
                OrderNumber: order.Number,
                UserId: order.UserId,
                OrderStatus: Enum.GetName(order.Status)!,
                Description: result.Value().Details));

            return default;
        }
    }
}