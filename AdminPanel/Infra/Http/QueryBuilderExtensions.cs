using System.Collections;
using Microsoft.AspNetCore.Http.Extensions;

namespace AdminPanel.Infra.Http;

public static class QueryBuilderExtensions
{
    public static QueryBuilder AddObject(this QueryBuilder queryBuilder, object? @object, string? prefix = null)
    {
        using var propertyInfoEnumerator = @object?.GetType()
            .GetProperties()
            .Where(prop => prop.GetMethod?.IsPublic == true)
            .GetEnumerator();

        if (propertyInfoEnumerator == null || propertyInfoEnumerator.MoveNext() == false)
        {
            if (prefix == null)
                throw new InvalidOperationException("Specified object has no properties and no default prefix given");

            if (@object is IEnumerable enumerable)
            {
                queryBuilder.Add(prefix, enumerable.Cast<object?>().Select(item => item?.ToString() ?? string.Empty));
                return queryBuilder;
            }

            queryBuilder.Add(prefix, @object?.ToString() ?? string.Empty);
            return queryBuilder;
        }

        do
        {
            var newPrefix = prefix == null
                ? propertyInfoEnumerator.Current.Name
                : $"{prefix}.{propertyInfoEnumerator.Current.Name}";

            queryBuilder.AddObject(propertyInfoEnumerator.Current.GetMethod!.Invoke(@object, null), newPrefix);
        } while (propertyInfoEnumerator.MoveNext());

        return queryBuilder;
    }
}