using Backend.Infra.Data;
using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Order.Infra.Configuration;

internal class DishInBasketConfiguration : IEntityTypeConfiguration<Domain.DishInBasket>
{
    public void Configure(EntityTypeBuilder<Domain.DishInBasket> builder)
    {
        builder.IsDependentEntity<Domain.DishInBasket, Entities.DishInBasket>(dish => new { dish.DishId, dish.UserId })
            .ToTable(nameof(BackendDbContext.DishesInBasket));
        
        builder.HasOne<Domain.Dish>(inBasket => inBasket.Dish)
            .WithMany()
            .HasForeignKey(inBasket => inBasket.DishId);

        builder.Property(inBasket => inBasket.DishId).HasColumnName(nameof(Entities.DishInBasket.DishId));
        builder.Property(inBasket => inBasket.UserId).HasColumnName(nameof(Entities.DishInBasket.UserId));
        builder.Property(inBasket => inBasket.Count).HasColumnName(nameof(Entities.DishInBasket.Count));
    }
}