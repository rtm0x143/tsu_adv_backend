using Backend.Common.Dtos;
using Backend.Converters;
using Backend.Features.Restaurant.Queries;
using Backend.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Backend.Controllers
{
    public partial class RestaurantController
    {
        [HttpGet("{id}")]
        public Task<ActionResult<RestaurantDto>> Get(Guid id, [FromServices] IGetById getById)
            => ExecuteRequest(getById, new(id));
    }
}

namespace Backend.Features.Restaurant.Queries
{
    public class GetById : IGetById
    {
        private readonly BackendDbContext _context;

        public GetById(BackendDbContext context) => _context = context;

        public async Task<OneOf<RestaurantDto, KeyNotFoundException>> Execute(GetByIdQuery request)
        {
            if (await _context.Restaurants.FindAsync(request.Id).ConfigureAwait(false)
                is Infra.Data.Entities.Restaurant restaurant)
                return restaurant.AdaptToDto();

            return new KeyNotFoundException(nameof(request.Id));
        }
    }
}