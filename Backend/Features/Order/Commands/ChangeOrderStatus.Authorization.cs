using Backend.Features.Order.Domain;
using Backend.Features.Order.Domain.Services;
using Backend.Features.Order.Domain.ValueTypes;
using Common.Domain;
using Common.Infra.Auth;
using Common.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Features.Order.Commands;

public static class ChangeOrderStatusPolicy
{
    public static AuthorizationPolicy Create(OrderStatus newStatus, ulong orderNumber) =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new ChangeOrderStatusRequirement(newStatus, orderNumber))
            .OrAbsolutePrivilege(builder
                => builder.RequireRole(nameof(CommonRoles.Admin)))
            .Build();
}

public record ChangeOrderStatusRequirement(OrderStatus NewStatus, ulong OrderNumber) : IAuthorizationRequirement;

public class ChangeOrderStatusHandler : AuthorizationHandler<ChangeOrderStatusRequirement>
{
    private readonly IRepository<OrderStatusState> _repository;
    public ChangeOrderStatusHandler(IRepository<OrderStatusState> repository) => _repository = repository;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ChangeOrderStatusRequirement requirement)
    {
        return new OrderStatusAuthService(_repository)
            .CanSet(requirement.NewStatus, requirement.OrderNumber, context.User)
            .ContinueWith(task =>
            {
                if (task.Result.IsT0) context.Succeed(requirement);
            });
    }
}