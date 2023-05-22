using System.Security.Claims;
using Backend.Features.Order.Domain.ValueTypes;
using Common.Domain;
using Common.Domain.Exceptions;
using Common.Domain.Utils;
using Common.Domain.ValueTypes;
using Common.Infra.Auth;
using OneOf;

namespace Backend.Features.Order.Domain.Services;

public class OrderStatusAuthService
{
    private readonly IRepository<OrderStatusState> _repository;
    public OrderStatusAuthService(IRepository<OrderStatusState> repository) => _repository = repository;

    public async Task<OneOf<EmptyResult, KeyNotFoundException, NotPermittedException>> CanSet(
        OrderStatus status, ulong orderNumber, ClaimsPrincipal interactPrincipal)
    {
        if (await _repository.QueryOne(
                query => query.Where(state => state.OrderNumber == orderNumber))
            is not OrderStatusState statusState)
            return new KeyNotFoundException(nameof(orderNumber));

        NotPermittedException? exception = null;
        switch (status)
        {
            case OrderStatus.Created:
                exception = new("Cant set default status");
                break;
            case OrderStatus.Canceled:
                if (!interactPrincipal.IsInRole(nameof(CommonRoles.Customer)) &&
                    !interactPrincipal.IsInRole(nameof(CommonRoles.Courier))) goto default;
                break;
            case OrderStatus.Kitchen:
            case OrderStatus.Packaging:
                if (!interactPrincipal.HasClaim(CommonClaimTypes.Manage, nameof(CommonManageTargets.Kitchen)))
                    goto default;
                break;
            case OrderStatus.Delivery:
                if (!interactPrincipal.HasClaim(CommonClaimTypes.Manage, nameof(CommonManageTargets.Delivery)))
                    goto default;
                break;
            case OrderStatus.Delivered:
                if (!interactPrincipal.HasClaim(CommonClaimTypes.Manage, nameof(CommonManageTargets.Delivery)))
                    goto default;

                if (statusState.OrderStatusLogs.Order().LastOrDefault(log => log.Status == OrderStatus.Delivery) is not
                        OrderStatusLog statusLog ||
                    statusLog.UserId != interactPrincipal.GetRequiredUserId())
                    exception = new("Delivery can be finished only by user who performed delivery");
                break;
            default:
                exception = new();
                break;
        }

        return exception == null ? new EmptyResult() : exception;
    }
}