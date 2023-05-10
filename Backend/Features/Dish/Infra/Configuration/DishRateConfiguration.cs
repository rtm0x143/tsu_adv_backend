using Backend.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Dish.Infra.Configuration;

public class DishRateConfiguration : IEntityTypeConfiguration<Domain.DishRate>
{
    public void Configure(EntityTypeBuilder<Domain.DishRate> builder)
    {
        builder.ToTable(nameof(BackendDbContext.DishRates))
            .HasKey(rate => new { rate.DishId, rate.UserId });

        builder.Property(rate => rate.Score)
            .HasConversion<RateScoreConverter>()
            .HasColumnName(nameof(Entities.DishRate.Value));
    }
}