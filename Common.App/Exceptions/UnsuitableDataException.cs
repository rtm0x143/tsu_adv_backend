using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Common.App.Exceptions;

public class UnsuitableDataException : Exception
{
    public virtual Dictionary<string, string> Problems { get; set; } = new();

    public UnsuitableDataException(string? message = default) : base(message)
    {
    }

    public ValidationProblemDetails ToProblemDetails() =>
        new(Problems.ToDictionary(pair => pair.Key, pair => new[] { pair.Value }));

    /// <summary>
    /// Creates new <see cref="UnsuitableDataException"/> from <see cref="IdentityResult"/>
    /// </summary>
    /// <exception cref="ArgumentException">When <paramref name="result"/>.<see cref="IdentityResult.Succeeded"/> was true</exception>
    public static UnsuitableDataException FromIdentityResult(IdentityResult result)
    {
        if (result.Succeeded) throw new ArgumentException("Result was successful", nameof(result));
        
        return new() { Problems = result.Errors.ToDictionary(e => e.Code, e => e.Description) };
    }
}