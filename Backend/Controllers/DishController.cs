using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backend.Common.Dtos;
using Backend.Infra.Data.Enums;
using Common.App;
using Common.App.Models.Results;
using Common.App.RequestHandlers;
using Microsoft.AspNetCore.Mvc;
using EmptyResult = Common.App.Models.Results.EmptyResult;

namespace Backend.Controllers;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DishSortingType
{
    Unspecified,
    NameAsc,
    NameDec,
    PriceAsc,
    PriceDec,
    RatingAsc,
    RatingDec
}

public sealed record GetDishesQuery(
    PaginationInfo<Guid> Pagination,
    Guid RestaurantId, 
    Guid MenuId = default,
    DishCategory[]? Categories = null,
    bool IsVegetarian = false,
    DishSortingType SortingType = DishSortingType.Unspecified);

public record DishDto(
        Guid Id,
        string Name,
        string? Photo,
        decimal Price,
        DishCategory Category,
        bool IsVegetarian,
        string? Description)
    : DishShortDto(Id, Name, Photo, Price, Category, IsVegetarian);

public record DishRichDto(
        Guid Id,
        string Name,
        string? Photo,
        decimal Price,
        DishCategory Category,
        bool IsVegetarian,
        string? Description,
        float Rating)
    : DishDto(Id, Name, Photo, Price, Category, IsVegetarian, Description);

public record DishCreationDto(
    Guid RestaurantId,
    string Name,
    string? Photo,
    decimal Price,
    DishCategory Category,
    string? Description,
    bool IsVegetarian);

public sealed record CreateDishCommand(DishCreationDto Dish) : IRequestWithException<IdResult>;

public sealed record RateDishCommand(float Score) : IRequestWithException<EmptyResult>;

public class DishController : CommonApiControllerBase<DishController>
{
    [HttpGet]
    public Task<ActionResult<DishShortDto[]>> Get([FromQuery] GetDishesQuery query)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public Task<ActionResult<DishRichDto>> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public Task<ActionResult<IdResult>> Create(DishCreationDto dish)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}")]
    public Task<ActionResult<IdResult>> Update(Guid id, DishCreationDto dish)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public Task<ActionResult<IdResult>> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}/rate/{score}")]
    public Task<ActionResult> Rate(Guid id, [Range(0, 10)] float score)
    {
        throw new NotImplementedException();
    }
}