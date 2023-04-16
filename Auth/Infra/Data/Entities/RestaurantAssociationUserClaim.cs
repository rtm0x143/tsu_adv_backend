using Common.Infra.Auth;
using Microsoft.AspNetCore.Identity;

namespace Auth.Infra.Data.Entities;

public class RestaurantAssociationUserClaim : IdentityUserClaim<Guid>
{
    public required Guid RestaurantId { get; set; }

    public override string? ClaimType { get; set; } = CommonClaimTypes.Restaurant;

    public override string? ClaimValue
    {
        get => RestaurantId.ToString();
        set => RestaurantId = (value == null ? Guid.Empty : Guid.Parse(value));
    }
}