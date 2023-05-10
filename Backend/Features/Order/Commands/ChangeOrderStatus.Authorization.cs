using Backend.Features.Order.Domain.Services;
using Backend.Features.Order.Domain.ValueTypes;
using Common.Infra.Auth;
using Common.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Features.Order.Commands;

public static class ChangeOrderStatusPolicy
{
    public static AuthorizationPolicy Instance { get; } = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireAssertion(context => context.Resource is OrderStatus status
                                     && OrderStatusAuthService.CanSet(status, context.User))
        .OrAbsolutePrivilege(builder
            => builder.RequireRole(Enum.GetName(CommonRoles.Admin)!))
        .Build();
}