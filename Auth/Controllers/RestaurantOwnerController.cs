using Common.App.Attributes;

namespace Auth.Controllers;

[VersioningApiRoute("restaurant-owner", OmitController = true)]
public partial class RestaurantOwnerController : AuthControllerBase<RestaurantOwnerController>
{
}