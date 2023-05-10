using OneOf;

namespace Backend.Features.Order.Domain;

public class DishInOrder
{
    public ulong OrderNumber { get; private set; }
    public Guid DishId { get; private set; }

    private Dish _dish = null!;

    public Dish Dish
    {
        get => _dish;
        private set
        {
            _dish = value;
            DishId = value.Id;
        }
    }

    public uint Count { get; private set; }

    private DishInOrder()
    {
    }

    /// <summary>
    /// Constructs existing <see cref="DishInOrder"/> model
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">When <paramref name="count"/> of dish in order less than 1</exception>
    public static OneOf<DishInOrder, ArgumentOutOfRangeException> Construct(ulong orderNumber, Dish dish, uint count)
    {
        if (count < 1) return new ArgumentOutOfRangeException($"{nameof(count)} can't less than one");
        return new DishInOrder
        {
            OrderNumber = orderNumber,
            Dish = dish,
            Count = count
        };
    }

    public static DishInOrder Clone(DishInOrder inOrder, ulong newOrderNumber) => new()
    {
        OrderNumber = newOrderNumber,
        Dish = inOrder.Dish,
        Count = inOrder.Count
    };
}