using Backend.Common.Dtos;
using Backend.Controllers;
using Backend.Features.Restaurant.Queries;
using Backend.Infra.Data;
using Backend.Infra.Data.Enums;
using Backend.Mappers;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    public partial class RestaurantController
    {
        [HttpGet("{id}/orders")]
        public Task<ActionResult<OrderShortDto[]>> Get(
            [FromServices] IGetRestaurantOrders getRestaurantOrders,
            Guid id,
            [FromQuery] uint pageSize,
            [FromQuery, ModelBinder(typeof(Base32LongModelBinder))]
            OrderNumber afterRecord = default,
            [FromQuery] OrderStatus[]? inStatus = null,
            [FromQuery] OrderSortingTypes sortingTypes = OrderSortingTypes.CreateTimeDec)
        {
            return getRestaurantOrders.Execute(new(id, new(pageSize, afterRecord), sortingTypes, inStatus))
                .ContinueWith<ActionResult<OrderShortDto[]>>(t => Ok(t.Result));
        }
    }
}

namespace Backend.Features.Restaurant.Queries
{
    public class GetRestaurantOrders : IGetRestaurantOrders
    {
        private readonly BackendDbContext _context;
        public GetRestaurantOrders(BackendDbContext context) => _context = context;

        public Task<OrderShortDto[]>
            Execute(GetRestaurantOrdersQuery query) // TODO : refactor ? maybe i can abstract it 
        {
            var queryable = _context.Orders.Where(o => o.Restaurant.Id == query.RestaurantId);
            if (query.InStatus != null)
                queryable = queryable.Where(o => query.InStatus.Contains(o.Status));
            if (query.Pagination.AfterRecord != default)
                queryable = queryable.Where(o => o.Number > query.Pagination.AfterRecord.Numeric);

            queryable = query.SortingTypes switch
            {
                OrderSortingTypes.CreateTimeAsc => queryable.OrderBy(o => o.CreatedTime),
                OrderSortingTypes.CreateTimeDec => queryable.OrderByDescending(o => o.CreatedTime),
                OrderSortingTypes.DeliveryTimeAsc => queryable.OrderBy(o => o.DeliveryTime),
                OrderSortingTypes.DeliveryTimeDec => queryable.OrderByDescending(o => o.DeliveryTime),
                _ => queryable
            };

            return queryable.Take((int)query.Pagination.PageSize)
                .ProjectToType<OrderShortDto>()
                .ToArrayAsync();
        }
    }
}