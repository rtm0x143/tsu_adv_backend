using System.Security.Claims;
using Backend.Features.Order.Domain.ValueTypes;
using Common.Infra.Auth;

namespace Backend.Features.Order.Domain.Services;

public static class OrderStatusAuthService
{
    public static bool CanSet(OrderStatus status, ClaimsPrincipal principal)
    {
        switch (status)
        {
            case OrderStatus.Created: return false;
            case OrderStatus.Canceled:
                return principal.IsInRole(Enum.GetName(CommonRoles.Customer)!)
                       || principal.IsInRole(Enum.GetName(CommonRoles.Courier)!);
            case OrderStatus.Kitchen:
            case OrderStatus.Packaging:
                return principal.HasClaim(CommonClaimTypes.Manage, nameof(CommonManageTargets.Kitchen));
            case OrderStatus.Delivery:
            case OrderStatus.Delivered:
                return principal.HasClaim(CommonClaimTypes.Manage, nameof(CommonManageTargets.Delivery));
            default:
                throw new ArgumentOutOfRangeException(nameof(status), status, null);
        }
    }
}