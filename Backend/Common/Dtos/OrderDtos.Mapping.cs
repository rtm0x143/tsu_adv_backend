using Backend.Converters;
using Backend.Infra.Data.Entities;
using Mapster;

namespace Backend.Common.Dtos;

internal class OrderShortDtoMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Order, OrderShortDto>()
            .GenerateMapper(MapType.Projection | MapType.Map);
    }
}

internal class OrderDtoMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Order, OrderDto>()
            .Map(dest => dest.Dishes,
                src => src.Dishes!.Select(dish => dish.Dish.AdaptToShortDto()).ToArray())
            .GenerateMapper(MapType.Projection | MapType.Map);
    }
}

internal class OrderNumberMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ulong, OrderNumber>()
            .ConstructUsing(number => new OrderNumber(number, OrderNumberFormatter.Encode(number)))
            .GenerateMapper(MapType.Map);

        config.NewConfig<OrderNumber, ulong>()
            .ConstructUsing(orderNumber => orderNumber.Numeric)
            .GenerateMapper(MapType.Map);
    }
}