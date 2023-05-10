using Backend.Features.Dish.Domain.ValueTypes;
using OneOf;

namespace Backend.Features.Dish.Domain.Services;

public class DishCreator
{
    private readonly bool _shouldGenerateId;
    public DishCreator(bool shouldGenerateId = true) => _shouldGenerateId = shouldGenerateId;

    public OneOf<Dish, Exception> CreateNew(
        string name,
        decimal price,
        DishCategory category,
        bool isVegetarian,
        Restaurant restaurant,
        string? photo,
        string? description)
    {
        return Dish.Construct(
            id: _shouldGenerateId ? Guid.NewGuid() : Guid.Empty,
            name, price, category, isVegetarian, restaurant,
            cachedRate: new Rate(default, 0),
            photo: Uri.TryCreate(photo, UriKind.Absolute, out var photoUri) ? photoUri : null, 
            description);
    }
}