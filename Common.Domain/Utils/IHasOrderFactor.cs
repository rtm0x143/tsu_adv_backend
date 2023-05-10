namespace Common.Domain.Utils;

/// <summary>
/// Interface to perform ordering of collection  
/// </summary>
public interface IHasOrderFactor : IComparable<IHasOrderFactor>
{
    int OrderFactor { get; }

    int IComparable<IHasOrderFactor>.CompareTo(IHasOrderFactor? other)
        => other == null ? 1 : OrderFactor.CompareTo(other.OrderFactor);
}