using System.Collections;

namespace Common.Domain.Utils;

public class HasOrderFactorComparer : IComparer<IHasOrderFactor>, IComparer, IComparer<object>
{
    /// <summary>
    /// This value is used if object specified to <see cref="IComparer.Compare"/> is not <see cref="IHasOrderFactor"/>
    /// </summary>
    public int DefaultOrderFactorValue { get; init; } = 0;

    public int Compare(object? x, object? y)
    {
        if (y == null) return 1;
        if (x == null) return -1;

        return ((x as IHasOrderFactor)?.OrderFactor ?? DefaultOrderFactorValue)
            .CompareTo((y as IHasOrderFactor)?.OrderFactor ?? DefaultOrderFactorValue);
    }

    public int Compare(IHasOrderFactor? x, IHasOrderFactor? y)
    {
        if (y == null) return 1;
        if (x == null) return -1;

        return x.OrderFactor.CompareTo(y.OrderFactor);
    }
}