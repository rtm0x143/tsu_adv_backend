using System.ComponentModel.DataAnnotations;
using Backend.Features.Dish.Commands;
using Backend.Features.Dish.Domain.Services;
using Backend.Features.Dish.Domain.ValueTypes;
using Backend.Features.Dish.Infra;
using Common.App.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace Backend.Controllers
{
    
    
    public partial class DishController
    {
        /// <summary>Set user's rate on specified dish</summary>
        /// <response code="404"></response>
        /// <response code="401"></response>
        /// <response code="403">When user can't rate specified dish</response>
        [Authorize]
        [HttpPut("{id}/rate/{score}")]
        public async Task<ActionResult> Rate(Guid id, [Range(0, 10)] float score, [FromServices] IRateDish rateDish)
        {
            if (!Guid.TryParse(GetUserId(), out var userId)) return InvalidTokenPayload();

            if (await AuthService.AuthorizeAsync(User, null, new CanRateDishRequirement(id))
                is { Succeeded: false })
                return Forbid();

            return await ExecuteRequest(rateDish, new(id, score, userId));
        }
    }
}

namespace Backend.Features.Dish.Commands
{
    public class RateDish : IRateDish
    {
        private readonly DishDbContext _context;
        private readonly IDishRatesRepository _dishRatesRepository;

        public RateDish(DishDbContext context, IDishRatesRepository dishRatesRepository)
        {
            _context = context;
            _dishRatesRepository = dishRatesRepository;
        }

        public async Task<OneOf<EmptyResult, Exception>> Execute(RateDishCommand command)
        {
            if (await _context.Dishes.FindAsync(command.DishId) is not Domain.Dish dish)
                return new KeyNotFoundException(nameof(command.DishId));

            var scoreResult = RateScore.Construct(command.Score);
            if (!scoreResult.Succeeded()) return scoreResult.Error();

            var ratingService = new RatingService(_dishRatesRepository);
            var rateDishResult = await ratingService.RateDish(dish, scoreResult.Value(), command.UserId);
            if (!rateDishResult.Succeeded()) return rateDishResult.Error();

            var entityEntry = _context.Entry(rateDishResult.Value());
            entityEntry.State = entityEntry.State == EntityState.Detached ? EntityState.Added : entityEntry.State;

            await _context.SaveChangesAsync();
            return default;
        }
    }
}