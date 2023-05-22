using System.Diagnostics.CodeAnalysis;
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

    /// <summary>
    /// Semantically better way to call <see cref="OneOf{T0,T1}.TryPickT0"/> where T0 represents valid result and T1 exception
    /// </summary>
    /// <returns><c>true</c> if <see cref="Succeeded{TValue,TException}"/> otherwise <c>false</c></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetValue<TValue, TException>(this OneOf<TValue, TException> oneOf,
        [NotNullWhen(true)] out TValue? value,
        [NotNullWhen(false)] out TException? exception)
        where TException : Exception
        => oneOf.TryPickT0(out value, out exception);
}