using System.Diagnostics.CodeAnalysis;
using Backend.Features.Dish.Domain.ValueTypes;
using Common.Domain.ValueTypes;
using OneOf;

namespace Backend.Features.Dish.Domain;

public class Dish
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = default!;

    private static bool _validateName(string name, [NotNullWhen(false)] out ArgumentException? exception)
    {
        if (name == string.Empty)
        {
            exception = new ArgumentException("Can't be empty string", nameof(name));
            return false;
        }

        exception = null;
        return true;
    }

    public OneOf<EmptyResult, ArgumentException> ChangeName(string name)
    {
        if (!_validateName(name, out var ex)) return ex;
        Name = name;
        return default;
    }

    public Uri? Photo { get; set; }

    public decimal Price { get; private set; }

    private static bool _validatePrice(decimal price, [NotNullWhen(false)] out ArgumentOutOfRangeException? exception)
    {
        if (price < 0)
        {
            exception = new ArgumentOutOfRangeException(nameof(price), "Price can't be below zero");
            return false;
        }

        exception = null;
        return true;
    }

    public OneOf<EmptyResult, ArgumentOutOfRangeException> ChangePrice(decimal price)
    {
        if (!_validatePrice(price, out var ex)) return ex;
        Price = price;
        return default;
    }

    public string? Description { get; set; }
    public DishCategory Category { get; set; }
    public bool IsVegetarian { get; set; }

    public Guid RestaurantId { get; private set; }

    private Restaurant _restaurant = null!;

    public Restaurant Restaurant
    {
        get => _restaurant;
        private set
        {
            _restaurant = value;
            RestaurantId = _restaurant.Id;
        }
    }

    public Rate CachedRate { get; set; } = default!;

    /// <summary>
    /// Constructs existing <see cref="Dish"/> model
    /// </summary>
    /// <returns><see cref="Dish"/> instance of exception if some arguments were invalid</returns>
    public static OneOf<Dish, Exception> Construct(
        Guid id,
        string name,
        decimal price,
        DishCategory category,
        bool isVegetarian,
        Restaurant restaurant,
        Rate cachedRate,
        Uri? photo,
        string? description)
    {
        if (!_validateName(name, out var nameEx)) return nameEx;
        if (!_validatePrice(price, out var priceEx)) return priceEx;

        return new Dish
        {
            Restaurant = restaurant,
            Category = category,
            Description = description,
            Id = id,
            Name = name,
            Photo = photo,
            CachedRate = cachedRate,
            IsVegetarian = isVegetarian,
            Price = price
        };
    }
}