using Common.App.Attributes;

namespace Auth.Controllers;

[VersioningApiRoute("restaurant-admin", OmitController = true)]
public partial class RestaurantAdminController : AuthControllerBase<RestaurantAdminController>
{
}