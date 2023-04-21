using Backend.Infra.Data.Enums;

namespace Backend.Common.Dtos;

public record DishShortDto(
    Guid Id,
    string Name,
    string? Photo,
    decimal Price,
    DishCategory Category,
    bool IsVegetarian);