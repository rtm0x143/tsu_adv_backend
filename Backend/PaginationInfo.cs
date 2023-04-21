using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Backend;

/// <summary>
/// Info to perform key-set pagination 
/// </summary>
/// <param name="AfterRecord">Last record in previous page or <c>null</c> if requested first page</param>
/// <param name="PageSize">Count of records to load</param>
/// <typeparam name="TId">Type of <paramref name="AfterRecord"/>> id</typeparam>
public record PaginationInfo<TId>(uint PageSize, TId? AfterRecord = default);