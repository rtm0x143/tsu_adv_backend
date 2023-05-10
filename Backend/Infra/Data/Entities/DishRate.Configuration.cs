using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain = Backend.Features.Dish.Domain;


namespace Backend.Infra.Data.Entities;

internal class DishRateConfiguration : IEntityTypeConfiguration<DishRate>
{
    public void Configure(EntityTypeBuilder<DishRate> builder)
    {
        builder.IsDependentEntity<DishRate, Domain.DishRate>(rate => new { rate.DishId, rate.UserId })
            .ToTable(nameof(BackendDbContext.DishRates));

        builder.HasOne<Dish>()
            .WithMany()
            .HasForeignKey(r => r.DishId);

        builder.Property(rate => rate.Value).HasColumnName(nameof(DishRate.Value));
    }
}