using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infra.Data.Entities;

internal class DishInCartConfiguration : IEntityTypeConfiguration<DishInCart>
{
    public void Configure(EntityTypeBuilder<DishInCart> builder)
    {
        builder.HasOne<Dish>(inCart => inCart.Dish)
            .WithMany()
            .HasForeignKey(inCart => inCart.DishId);

        builder.HasKey(dish => new { dish.DishId, dish.UserId });
    }
}