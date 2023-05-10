using Backend.Converters;
using Backend.Features.Dish.Queries;
using Backend.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Backend.Controllers
{
    public partial class DishController
    {
        [HttpGet("{id}")]
        public Task<ActionResult<DishDto>> Get(Guid id, [FromServices] IGetById getById)
            => ExecuteRequest(getById, new(id));
    }
}

namespace Backend.Features.Dish.Queries
{
    public class GetById : IGetById
    {
        private readonly BackendDbContext _context;
        public GetById(BackendDbContext context) => _context = context;

        public Task<OneOf<DishDto, KeyNotFoundException>> Execute(GetByIdQuery query)
        {
            return _context.Dishes.FirstOrDefaultAsync(d => d.Id == query.Id)
                .ContinueWith<OneOf<DishDto, KeyNotFoundException>>(t => t.Result == null
                    ? new KeyNotFoundException(nameof(query.Id))
                    : t.Result.AdaptToDto());
        }
    }
}