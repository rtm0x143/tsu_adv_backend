using System.ComponentModel;
using Backend.Features.Restaurant.Common;
using Backend.Features.Restaurant.Queries;
using Backend.Infra.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    public partial class RestaurantController
    {
        [HttpGet("page")]
        public Task<ActionResult<RestaurantDto[]>> GetPaged([FromQuery] GetRestaurantsQuery query,
            [FromServices] IGetRestaurants getRestaurants)
        {
            return getRestaurants.Execute(query)
                .ContinueWith<ActionResult<RestaurantDto[]>>(t => Ok(t.Result));
        }
    }
}

namespace Backend.Features.Restaurant.Queries
{
    public class GetRestaurants : IGetRestaurants
    {
        private readonly BackendDbContext _context;
        public GetRestaurants(BackendDbContext context) => _context = context;

        public Task<RestaurantDto[]> Execute(GetRestaurantsQuery query)
        {
            return _context.Restaurants
                .Where(restaurant => restaurant.Id > query.Pagination.AfterRecord)
                .Take((int)query.Pagination.PageSize)
                .ProjectToType<RestaurantDto>()
                .ToArrayAsync();
        }
    }
}