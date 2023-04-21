using System.Runtime.CompilerServices;
using OneOf;

namespace Common.App.Utils;

public static class OneOfExtensions
{
    /// <summary>
    /// Semantically better way to call <see cref="OneOf{T0,T1}.IsT0"/> where T0 represents valid result and T1 exception
    /// </summary>
    /// <returns><see cref="OneOf{T0,T1}.IsT0"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Succeeded<TValue, TException>(this OneOf<TValue, TException> oneOf)
        where TException : Exception
        => oneOf.IsT0;

    /// <summary>
    /// Semantically better way to call <see cref="OneOf{T0,T1}.AsT0"/> where T0 represents valid result and T1 exception
    /// </summary>
    /// <exception cref="InvalidOperationException">When <see cref="OneOf{T0,T1}.IsT0"/> is false</exception>
    /// <returns><see cref="OneOf{T0,T1}.AsT0"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue Value<TValue, TException>(this OneOf<TValue, TException> oneOf)
        where TException : Exception
        => oneOf.AsT0;
    
    /// <summary>
    /// Semantically better way to call <see cref="OneOf{T0,T1}.AsT1"/> where T0 represents valid result and T1 exception
    /// </summary>
    /// <exception cref="InvalidOperationException">When <see cref="OneOf{T0,T1}.IsT1"/> is false</exception>
    /// <returns><see cref="OneOf{T0,T1}.AsT1"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TException Error<TValue, TException>(this OneOf<TValue, TException> oneOf)
        where TException : Exception
        => oneOf.AsT1;
}