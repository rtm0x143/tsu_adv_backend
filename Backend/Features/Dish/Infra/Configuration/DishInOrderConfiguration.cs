using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Dish.Infra.Configuration;

public class DishInOrderConfiguration : IEntityTypeConfiguration<Dish.Domain.DishInOrder>
{
    public void Configure(EntityTypeBuilder<Domain.DishInOrder> builder)
    {
        builder.IsDependentEntity<Dish.Domain.DishInOrder, Order.Domain.DishInOrder>(
                inOrder => new { inOrder.DishId, inOrder.OrderNumber })
            .ToTable(Entities.DishInOrderConfiguration.TableName);

        builder.Property(inOrder => inOrder.DishId).HasColumnName(nameof(Entities.DishInOrder.DishId));
        builder.Property(inOrder => inOrder.OrderNumber).HasColumnName(nameof(Entities.DishInOrder.OrderNumber));

        builder.HasOne(inOrder => inOrder.Order)
            .WithMany()
            .HasForeignKey(inOrder => inOrder.OrderNumber);

        builder.Navigation(inOrder => inOrder.Order).AutoInclude();
    }
}