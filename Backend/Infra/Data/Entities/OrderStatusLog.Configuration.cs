using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infra.Data.Entities;

internal class OrderStatusLogConfiguration : IEntityTypeConfiguration<OrderStatusLog>
{
    internal const string TableName = "OrderStatusLog";

    public void Configure(EntityTypeBuilder<OrderStatusLog> builder)
    {
        builder.IsDependentEntity<OrderStatusLog, Features.Order.Domain.OrderStatusLog>(log => log.Id)
            .ToTable(TableName);

        builder.Property(log => log.OrderNumber).HasColumnName(nameof(OrderStatusLog.OrderNumber));
        builder.Property(log => log.UserId).HasColumnName(nameof(OrderStatusLog.UserId));
        builder.Property(log => log.Status).HasColumnName(nameof(OrderStatusLog.Status));
        builder.Property(log => log.Details).HasColumnName(nameof(OrderStatusLog.Details));
        builder.Property(log => log.CreatedTime).HasColumnName(nameof(OrderStatusLog.CreatedTime));
    }
}