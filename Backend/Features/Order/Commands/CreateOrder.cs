using Backend.Common.Dtos;
using Backend.Converters;
using Backend.Features.Basket.Commands;
using Backend.Features.Order.Commands;
using Backend.Features.Order.Common;
using Backend.Features.Order.Domain.Services;
using Backend.Features.Order.Infra;
using Common.App.Dtos;
using Common.App.Utils;
using Common.Domain.ValueTypes;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Backend.Controllers
{
    public partial class OrderController
    {
        public record CreateOrderDto(string Address, DateTime DeliveryTime);


        /// <summary>
        /// Create new order from calling user's basket
        /// </summary>
        /// <response code="404"></response>
        /// <response code="401"></response>
        /// <response code="403">When user isn't customer</response>
        [HttpPost]
        [Authorize(Roles = nameof(CommonRoles.Customer))]
        public Task<ActionResult<OrderNumberResult>> Create(
            [FromBody] CreateOrderDto dto,
            [FromServices] ICreateOrder createOrder,
            [FromQuery] bool clearBasket = true)
        {
            if (!Guid.TryParse(GetUserId(), out var userId))
                return Task.FromResult<ActionResult<OrderNumberResult>>(InvalidTokenPayload());

            return createOrder.Execute(new(dto.Address, dto.DeliveryTime, userId, clearBasket))
                .ContinueWith<ActionResult<OrderNumberResult>>(t => t.Result.Succeeded()
                    ? Ok(new OrderNumberResult(t.Result.Value().Id))
                    : ExceptionsDescriber.Describe(t.Result.Error()));
        }
    }
}

namespace Backend.Features.Order.Commands
{
    public class CreateOrder : ICreateOrder
    {
        private readonly OrderDbContext _context;
        private readonly IClearBasket _clearBasket;

        public CreateOrder(OrderDbContext context, IClearBasket clearBasket)
        {
            _context = context;
            _clearBasket = clearBasket;
        }

        public async Task<OneOf<IdResult<ulong>, Exception>> Execute(CreateOrderCommand command)
        {
            var inBasket = await _context.DishesInBasket
                .Include(inBasket => inBasket.Dish)
                .Where(inBasket => inBasket.UserId == command.UserId)
                .Select(DishInBasketMapper.ProjectToDishInOrderDto)
                .ToArrayAsync();

            var orderResult = await new OrderCreator(_context)
                .CreateNew(inBasket, command.Address, command.UserId, command.DeliveryTime);
            if (!orderResult.Succeeded()) return orderResult.Error();

            var entry = _context.Orders.Add(orderResult.Value());
            if (command.ClearBasket) await _clearBasket.Execute(new(command.UserId));

            await _context.SaveChangesAsync();
            return new IdResult<ulong>(entry.Entity.Number);
        }
    }
}