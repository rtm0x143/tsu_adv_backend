﻿using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Common.Infra.Dal;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortType
{
    Asc,
    Dec
}

public static class QueryableExtensions
{
    public static IQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> queryable,
        Expression<Func<TSource, TKey>> keySelector, SortType type) => type switch
    {
        SortType.Asc => queryable.OrderBy(keySelector),
        SortType.Dec => queryable.OrderByDescending(keySelector),
        _ => queryable
    };

    public static IQueryable<T> Option<T>(this IQueryable<T> queryable, bool condition,
        Func<IQueryable<T>, IQueryable<T>> option)
    {
        return condition ? option(queryable) : queryable;
    }
}