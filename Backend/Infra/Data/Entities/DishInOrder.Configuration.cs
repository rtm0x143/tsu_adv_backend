using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infra.Data.Entities;

internal class DishInOrderConfiguration : IEntityTypeConfiguration<DishInOrder>
{
    public void Configure(EntityTypeBuilder<DishInOrder> builder)
    {
        builder.HasOne<Dish>(inOrder => inOrder.Dish)
            .WithMany()
            .HasForeignKey(inOrder => inOrder.DishId)
            .HasPrincipalKey(dish => dish.Id);

        builder.HasOne<Order>(inOrder => inOrder.Order)
            .WithMany(order => order.Dishes)
            .HasForeignKey(inOrder => inOrder.OrderNumber)
            .HasPrincipalKey(order => order.Number);

        builder.HasKey(inOrder => new { inOrder.DishId, inOrder.OrderNumber });
    }
}