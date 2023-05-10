using OneOf;

namespace Backend.Features.Dish.Domain.ValueTypes;

public record struct RateScore
{
    public const float MaxValue = 10f;
    public const float MinValue = 0f;

    public readonly float Value;

    private RateScore(float value) => Value = value;

    public static OneOf<RateScore, ArgumentOutOfRangeException> Construct(float scoreValue) =>
        scoreValue is < MinValue or > MaxValue
            ? new ArgumentOutOfRangeException(nameof(scoreValue), $"Not satisfy value range [{MinValue}; {MaxValue}]")
            : new RateScore(scoreValue);
}