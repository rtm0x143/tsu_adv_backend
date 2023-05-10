using System.ComponentModel.DataAnnotations;
using Backend.Features.Dish.Domain.ValueTypes;

namespace Backend.Common.Dtos;

public record DishShortDto(
    Guid Id,
    string Name,
    [Url] string? Photo,
    [Range(0, double.MaxValue)] decimal Price,
    DishCategory Category,
    bool IsVegetarian);

public record DishCreationDto(
    string Name,
    [Url] string? Photo,
    [Range(0, double.MaxValue)] decimal Price,
    DishCategory Category,
    string? Description,
    bool IsVegetarian);