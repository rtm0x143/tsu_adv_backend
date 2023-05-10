using Backend.Common.Dtos;
using Backend.Features.Order.Domain.ValueTypes;
using Backend.Features.Order.Queries;
using Common.Infra.Auth;
using Common.Infra.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Backend.Controllers;

public partial class OrderController
{
    /// <summary>
    /// Get orders with huge query
    /// </summary>
    /// <response code="401"></response>
    /// <response code="403">When user can't view these orders</response>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<OrderShortDto[]>> Get([FromQuery] GetOrdersQuery query,
        [FromServices] IGetOrders getOrders)
    {
        if (await AuthService.AuthorizeAsync(User,
                resource: null,
                policy: PersonalDataInRestaurantPolicy.CreateForRead(query.UserId, query.RestaurantId))
            is { Succeeded: false })
            return Forbid();

        return Ok(await getOrders.Execute(query));
    }

    /// <summary>
    /// Get current user's processing now orders 
    /// </summary>
    /// <response code="401"></response>
    /// <response code="403">When user isn't customer</response>
    [Authorize(Roles = nameof(CommonRoles.Customer))]
    [HttpGet("processing")]
    public Task<ActionResult<OrderShortDto[]>> Get([FromServices] IGetOrders getOrders)
    {
        if (!Guid.TryParse(GetUserId(), out var userId))
            return Task.FromResult<ActionResult<OrderShortDto[]>>(InvalidTokenPayload());

        return getOrders.Execute(new(
                Pagination: new(int.MaxValue),
                UserId: userId,
                SortType: SortType.Dec,
                SortFactor: OrderSortFactors.DeliveryTime,
                InStatus: OrderStatuses.ProcessingOrderStatuses))
            .ContinueWith<ActionResult<OrderShortDto[]>>(t => Ok(t.Result));
    }
}