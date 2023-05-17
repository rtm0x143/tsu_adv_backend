using Backend.Common.Dtos;
using Backend.Converters;
using Backend.Features.Order.Queries;
using Backend.Infra.Data;
using Common.App.Dtos;
using Common.App.Utils;
using Common.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Backend.Controllers
{
    public partial class OrderController
    {
        /// <summary>
        /// Get information about the order
        /// </summary>
        /// <response code="404"></response>
        /// <response code="401"></response>
        /// <response code="403">When user has no permissions to read that data</response>
        [Authorize]
        [HttpGet("{number}")]
        public async Task<ActionResult<OrderDto>> GetByNumber(
            [FromRoute] OrderNumber number,
            [FromServices] IGetByNumber getByNumber)
        {
            var result = await getByNumber.Execute(new(number.Numeric));
            if (!result.Succeeded()) return ExceptionsDescriber.Describe(result.Error());
            var order = result.Value();

            var authResult = await AuthService
                .AuthorizeAsync(User, null, ActionOnPersonalDataRequirement.Read(order.UserId));
            return authResult.Succeeded ? Ok(order) : Forbid();
        }
    }
}

namespace Backend.Features.Order.Queries
{
    public class GetByNumber : IGetByNumber
    {
        private readonly BackendDbContext _context;
        public GetByNumber(BackendDbContext context) => _context = context;

        public Task<OneOf<OrderDto, Exception>> Execute(GetByNumberQuery query)
        {
            return _context.Orders.Where(order => order.Number == query.Number)
                .Include(order => order.Dishes!)
                .ThenInclude(inOrder => inOrder.Dish)
                .Include(order => order.Restaurant)
                .Select(OrderMapper.ProjectToDto)
                .FirstOrDefaultAsync()
                .ContinueWith<OneOf<OrderDto, Exception>>(t => t.Result == null
                    ? new KeyNotFoundException(nameof(query.Number))
                    : t.Result);
        }
    }
}