using Backend.Features.Dish.Domain.ValueTypes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Backend.Features.Dish.Infra.Configuration;

public class RateScoreConverter : ValueConverter<RateScore, float>
{
    public RateScoreConverter() : base(
        score => score.Value,
        scoreValue => RateScore.Construct(scoreValue)
            .Match(score => score, ex => default))
    {
    }
}