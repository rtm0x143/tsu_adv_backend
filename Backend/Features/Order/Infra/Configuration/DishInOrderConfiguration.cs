using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Order.Infra.Configuration;

public class DishInOrderConfiguration : IEntityTypeConfiguration<Domain.DishInOrder>
{
    public void Configure(EntityTypeBuilder<Domain.DishInOrder> builder)
    {
        builder.HasKey(d => new { d.DishId, d.OrderNumber });
        builder.ToTable(Entities.DishInOrderConfiguration.TableName);

        builder.HasOne<Domain.Dish>(inBasket => inBasket.Dish)
            .WithMany()
            .HasForeignKey(inBasket => inBasket.DishId);

        builder.Property(d => d.Count)
            .HasColumnName(nameof(Entities.DishInOrder.Count))
            .IsRequired();
        builder.Property(d => d.DishId)
            .HasColumnName(nameof(Entities.DishInOrder.DishId))
            .IsRequired();
        builder.Property(d => d.OrderNumber)
            .HasColumnName(nameof(Entities.DishInOrder.OrderNumber))
            .IsRequired();
    }
}