using Backend.Common.Dtos;
using Mapster;

namespace Backend.Features.Dish.Commands;

public class DishCreationDtoMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DishCreationDto, Domain.Dish>()
            .Ignore(d => d.Id,
                d => d.Name,
                d => d.Price,
                d => d.RestaurantId,
                d => d.CachedRate)
            .GenerateMapper(MapType.MapToTarget);
    }
}