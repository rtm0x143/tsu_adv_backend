using Backend.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Order.Domain
{
    public partial class Order
    {
        internal const string DishesBackedFieldName = nameof(_dishes);
        internal const string StatusLogsBackedFieldName = nameof(_statusLogs);
    }
}

namespace Backend.Features.Order.Infra.Configuration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Domain.Order>
    {
        public void Configure(EntityTypeBuilder<Domain.Order> builder)
        {
            builder.ToTable(nameof(BackendDbContext.Orders))
                .HasKey(order => order.Number);
            
            builder.Property(order => order.Address).HasColumnName(nameof(Entities.Order.Address));
            builder.Property(order => order.Price).HasColumnName(nameof(Entities.Order.Price));
            builder.Property(order => order.Status).HasColumnName(nameof(Entities.Order.Status));
            builder.Property(order => order.RestaurantId).HasColumnName(nameof(Entities.Order.RestaurantId));
            builder.Property(order => order.UserId).HasColumnName(nameof(Entities.Order.UserId));
            builder.Property(order => order.CreatedTime).HasColumnName(nameof(Entities.Order.CreatedTime));
            builder.Property(order => order.DeliveryTime).HasColumnName(nameof(Entities.Order.DeliveryTime));

            builder.Navigation(order => order.Dishes)
                .HasField(Domain.Order.DishesBackedFieldName)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.Navigation(order => order.StatusLogs)
                .HasField(Domain.Order.StatusLogsBackedFieldName)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}