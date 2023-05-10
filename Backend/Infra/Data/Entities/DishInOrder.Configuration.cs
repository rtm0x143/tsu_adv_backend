using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infra.Data.Entities;

internal class DishInOrderConfiguration : IEntityTypeConfiguration<DishInOrder>
{
    internal const string TableName = nameof(DishInOrder);

    public void Configure(EntityTypeBuilder<DishInOrder> builder)
    {
        builder.IsDependentEntity<DishInOrder, Features.Order.Domain.DishInOrder>(
                inOrder => new { inOrder.DishId, inOrder.OrderNumber })
            .ToTable(TableName);

        builder.HasOne<Dish>(inOrder => inOrder.Dish)
            .WithMany()
            .HasForeignKey(inOrder => inOrder.DishId)
            .HasPrincipalKey(dish => dish.Id);

        builder.HasOne<Order>(inOrder => inOrder.Order)
            .WithMany(order => order.Dishes)
            .HasForeignKey(inOrder => inOrder.OrderNumber)
            .HasPrincipalKey(order => order.Number);

        builder.Property(d => d.Count).HasColumnName(nameof(DishInOrder.Count));
        builder.Property(d => d.DishId).HasColumnName(nameof(DishInOrder.DishId));
        builder.Property(d => d.OrderNumber).HasColumnName(nameof(DishInOrder.OrderNumber));
    }
}