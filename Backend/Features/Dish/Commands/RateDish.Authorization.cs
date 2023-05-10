using Backend.Features.Dish.Domain.Services;
using Common.App.Utils;
using Common.Domain;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Backend.Features.Dish.Commands;

public record CanRateDishRequirement(Guid DishId) : IAuthorizationRequirement;

public class CanRateDishAuthorizationHandler : AuthorizationHandler<CanRateDishRequirement>
{
    private readonly RatingAuthService _ratingAuthService;
    private ClaimsIdentityOptions _claimsOptions;

    public CanRateDishAuthorizationHandler(IRepository<Domain.Dish> dishRepository,
        IRepository<Domain.DishInOrder> dishInOrderRepository,
        IOptions<ClaimsIdentityOptions> claimsOptions)
    {
        _ratingAuthService = new RatingAuthService(dishRepository, dishInOrderRepository);
        _claimsOptions = claimsOptions.Value;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        CanRateDishRequirement requirement)
    {
        if (!context.User.TryFindFirstGuid(_claimsOptions.UserIdClaimType, out var userId))
            return Task.CompletedTask;

        return _ratingAuthService.CanUserRateDish(requirement.DishId, userId)
            .ContinueWith(t =>
            {
                if (t.Result.IsT0)
                    context.Succeed(requirement);
                else
                    context.Fail(this.FailureFromException((t.Result.Value as Exception)!));
            });
    }
}