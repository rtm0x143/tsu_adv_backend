using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infra.Data.Entities;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.IsDependentEntity<Order, Features.Order.Domain.Order>(order => order.Number)
            .ToTable(nameof(BackendDbContext.Orders));

        builder.Property(order => order.Address).HasColumnName(nameof(Order.Address));
        builder.Property(order => order.Price).HasColumnName(nameof(Order.Price));
        builder.Property(order => order.Status).HasColumnName(nameof(Order.Status));
        builder.Property(order => order.RestaurantId).HasColumnName(nameof(Order.RestaurantId));
        builder.Property(order => order.UserId).HasColumnName(nameof(Order.UserId));
        builder.Property(order => order.CreatedTime).HasColumnName(nameof(Order.CreatedTime));
        builder.Property(order => order.DeliveryTime).HasColumnName(nameof(Order.DeliveryTime));
    }
}