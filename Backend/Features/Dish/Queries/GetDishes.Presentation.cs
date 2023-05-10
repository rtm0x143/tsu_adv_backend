using Backend.Features.Dish.Queries;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Backend.Controllers;

public partial class DishController
{
    [HttpGet]
    public Task<ActionResult<DishDto[]>> Get([FromQuery] GetDishesQuery query,
        [FromServices] IGetDishes getDishes)
        => ExecuteRequest(getDishes, query);
}