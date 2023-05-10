using Common.Infra.Auth;
using Common.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Features.Order.Queries;

public static class PersonalDataInRestaurantPolicy
{
    public static AuthorizationPolicy Create(Guid dataOwnerId, Guid restaurantId, string actionType)
        => new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(InRestaurantPolicy.CreateRequirement(restaurantId))
            .OrAbsolutePrivilege(builder
                => builder.AddRequirements(new ActionOnPersonalDataRequirement(dataOwnerId, actionType)))
            .Build();

    public static AuthorizationPolicy CreateForRead(Guid dataOwnerId, Guid restaurantId)
        => Create(dataOwnerId, restaurantId, Enum.GetName(CommonActionType.Read)!);

    public static AuthorizationPolicy CreateForChange(Guid dataOwnerId, Guid restaurantId)
        => Create(dataOwnerId, restaurantId, Enum.GetName(CommonActionType.Change)!);
}