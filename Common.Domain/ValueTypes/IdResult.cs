namespace Common.Domain.ValueTypes;

public record IdResult(Guid Id);
public record IdResult<TId>(TId Id);
