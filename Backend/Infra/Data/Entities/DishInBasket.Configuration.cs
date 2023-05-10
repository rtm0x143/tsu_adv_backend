using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infra.Data.Entities;

internal class DishInCartConfiguration : IEntityTypeConfiguration<DishInBasket>
{
    public void Configure(EntityTypeBuilder<DishInBasket> builder)
    {
        builder.ToTable(nameof(BackendDbContext.DishesInBasket))
            .HasKey(inBasket => new { inBasket.DishId, inBasket.UserId });

        builder.HasOne<Dish>(inBasket => inBasket.Dish)
            .WithMany()
            .HasForeignKey(inBasket => inBasket.DishId);

        builder.Property(inBasket => inBasket.DishId).HasColumnName(nameof(DishInBasket.DishId));
        builder.Property(inBasket => inBasket.UserId).HasColumnName(nameof(DishInBasket.UserId));
        builder.Property(inBasket => inBasket.Count).HasColumnName(nameof(DishInBasket.Count));
    }
}