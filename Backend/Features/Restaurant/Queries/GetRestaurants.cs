using Backend.Common.Dtos;
using Backend.Converters;
using Backend.Features.Restaurant.Common;
using Backend.Features.Restaurant.Queries;
using Backend.Infra.Data;
using Common.Infra.Dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    public partial class RestaurantController
    {
        /// <summary>
        /// Query restaurants paged 
        /// </summary>
        [HttpGet]
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
            return _context.Restaurants.Option(
                    condition: query.Pagination.AfterRecord != default,
                    option: queryable => queryable.Where(restaurant => restaurant.Id > query.Pagination.AfterRecord))
                .OrderBy(restaurant => restaurant.Id)
                .Take(query.Pagination.PageSize)
                .Select(RestaurantMapper.ProjectToDto)
                .AsNoTracking()
                .ToArrayAsync();
        }
    }
}