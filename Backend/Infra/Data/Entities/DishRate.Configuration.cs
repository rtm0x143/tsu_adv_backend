using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infra.Data.Entities;

internal class DishRateConfiguration : IEntityTypeConfiguration<DishRate>
{
    public void Configure(EntityTypeBuilder<DishRate> builder)
    {
        builder.HasNoKey()
            .HasOne<Dish>()
            .WithMany()
            .HasForeignKey(r => r.DishId);
    }
}