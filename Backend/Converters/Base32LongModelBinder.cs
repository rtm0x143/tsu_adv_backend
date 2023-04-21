using Backend.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.Mappers;

public class Base32LongModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext.ModelType != typeof(long)
            && bindingContext.ModelType != typeof(ulong)
            && bindingContext.ModelType != typeof(OrderNumber)) return Task.CompletedTask;

        var values = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        var validValues = new List<object>(values.Length);
        foreach (var value in values)
        {
            if (!Base32Converter.TryToLong(value, out var result))
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid value format");
            else
                validValues.Add(bindingContext.ModelType == typeof(OrderNumber)
                    ? new OrderNumber(result, value)
                    : result);
        }

        bindingContext.Result = validValues.Count == 0
            ? ModelBindingResult.Failed()
            : ModelBindingResult.Success(validValues.Count == 1 ? validValues.First() : validValues);

        return Task.CompletedTask;
    }
}