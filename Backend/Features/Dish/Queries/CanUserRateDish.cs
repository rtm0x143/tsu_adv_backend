using Backend.Features.Dish.Domain.Services;
using Backend.Features.Dish.Queries;
using Common.Domain;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Backend.Controllers
{
    public partial class DishController
    {
        /// <summary>Check if calling user can rate specified dish</summary>
        /// <response code="404"></response>
        /// <response code="401"></response>
        /// <response code="403">If user not allowed rate dish</response>
        /// <response code="200">If user allowed to rate dish</response>
        [Authorize(Roles = nameof(CommonRoles.Customer))]
        [HttpGet("{id}/rate/permitted")]
        public Task<ActionResult<bool>> CanRateDish(Guid id, [FromServices] ICanUserRateDish canUserRateDish)
        {
            if (Guid.TryParse(GetUserId(), out var userId))
                return Task.FromResult<ActionResult<bool>>(InvalidTokenPayload());

            return ExecuteRequest(canUserRateDish, new(id, userId));
        }
    }
}

namespace Backend.Features.Dish.Queries
{
    public class CanUserRateDish : ICanUserRateDish
    {
        private readonly RatingAuthService _ratingAuthService;

        public CanUserRateDish(
            IRepository<Domain.Dish> distRepository,
            IRepository<Domain.DishInOrder> inOrderRepository)
            => _ratingAuthService = new(distRepository, inOrderRepository);

        public Task<OneOf<bool, KeyNotFoundException>> Execute(CanUserRateDishQuery query)
        {
            return _ratingAuthService.CanUserRateDish(query.DishId, query.UserId).ContinueWith(
                task => task.Result.Match<OneOf<bool, KeyNotFoundException>>(
                    _ => true,
                    _ => false,
                    exception => exception));
        }
    }
}