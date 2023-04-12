namespace Common.App.Models.Results;

public record IdResult(Guid Id)
{
    /// <summary>
    /// Empty constructor for mapping needs
    /// </summary>
    public IdResult() : this(Guid.Empty) { }
}
