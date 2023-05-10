using Backend.Infra.Data.Entities;
using Mapster;

namespace Backend.Common.Dtos;

internal class DishShortDtoMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Dish, DishShortDto>()
            .GenerateMapper(MapType.Map | MapType.Projection);

        config.NewConfig<DishShortDto, Dish>()
            .GenerateMapper(MapType.MapToTarget);
    }
}