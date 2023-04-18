using System.Text.Json.Serialization;

namespace Common.Infra.Auth;

/// <summary> Preset widely used role names </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommonRoles
{
    Customer,
    Courier,
    Manager,
    Cook,
    /// <summary>
    /// User who is granted to manipulate Restaurant's data
    /// </summary>
    RestaurantAdmin,
    /// <summary>
    /// User who created Restaurant
    /// </summary>
    RestaurantOwner,
    Admin
}