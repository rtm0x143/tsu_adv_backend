using Backend.Common.Dtos;
using Backend.Features.Order.Commands;
using Backend.Features.Order.Domain.Services;
using Backend.Features.Order.Infra;
using Common.App.Utils;
using Common.Domain.ValueTypes;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Backend.Controllers
{
    /// <param name="DeliveryTime"></param>
    /// <param name="Address">In not specified use calling customer address</param>
    public record RepeatOrderDto(DateTime DeliveryTime, string? Address = null);

    public partial class OrderController
    {
        /// <summary>
        /// Repeat existing order 
        /// </summary>
        /// <response code="404"></response>
        /// <response code="401"></response>
        /// <response code="403">When user isn't customer</response>
        [Authorize(Roles = nameof(CommonRoles.Customer))]
        [HttpPost("{number}/repeat")]
        public Task<ActionResult<IdResult>> Repeat(
            [FromRoute] OrderNumber number,
            [FromBody] RepeatOrderDto dto,
            [FromServices] IRepeatOrder repeatOrder)
        {
            if (!Guid.TryParse(GetUserId(), out var userId))
                return Task.FromResult<ActionResult<IdResult>>(InvalidTokenPayload());

            return repeatOrder.Execute(new(number.Numeric, dto.DeliveryTime, dto.Address, userId))
                .ContinueWith<ActionResult<IdResult>>(t => t.Result.Succeeded()
                    ? Ok(t.Result.Value())
                    : ExceptionsDescriber.Describe(t.Result.Error()));
        }
    }
}

namespace Backend.Features.Order.Commands
{
    public class RepeatOrder : IRepeatOrder
    {
        private readonly OrderDbContext _context;
        public RepeatOrder(OrderDbContext context) => _context = context;

        public async Task<OneOf<OrderNumber, Exception>> Execute(RepeatOrderCommand command)
        {
            if (await _context.GetOrderByNumber(command.OrderNumber) is not Domain.Order orderModel)
                return new KeyNotFoundException(nameof(command.OrderNumber));

            return await new OrderCreator(_context)
                .RepeatOrder(orderModel, command.DeliveryTime, command.Address, command.UserId)
                .ContinueWith(t => t.Result.MapT0(order => new OrderNumber(order.Number)));
        }
    }
}