﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Common.App.Dtos;

/// <summary>
/// Info to perform key-set pagination 
/// </summary>
/// <param name="AfterRecord">Last record in previous page or <c>null</c> if requested first page</param>
/// <param name="PageSize">Count of records to load</param>
/// <typeparam name="TId">Type of <paramref name="AfterRecord"/>> id</typeparam>
public record PaginationInfo<TId>([param: Range(1, int.MaxValue)] int PageSize,
    [param: ValidateNever] TId? AfterRecord = default);