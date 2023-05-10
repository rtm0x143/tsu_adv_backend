using System.Text.Json.Serialization;

namespace Backend.Features.Dish.Domain.ValueTypes;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DishCategory
{
    Wok,
    Pizza,
    Soup,
    Desert,
    Drink
}