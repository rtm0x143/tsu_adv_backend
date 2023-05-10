using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Order.Infra.Configuration;

internal class OrderStatusLogConfiguration : IEntityTypeConfiguration<Domain.OrderStatusLog>
{
    public void Configure(EntityTypeBuilder<Domain.OrderStatusLog> builder)
    {
        builder.ToTable(Entities.OrderStatusLogConfiguration.TableName)
            .HasKey(log => log.Id);

        builder.Property(log => log.OrderNumber).HasColumnName(nameof(Entities.OrderStatusLog.OrderNumber));
        builder.Property(log => log.UserId).HasColumnName(nameof(Entities.OrderStatusLog.UserId));
        builder.Property(log => log.Status).HasColumnName(nameof(Entities.OrderStatusLog.Status));
        builder.Property(log => log.Details).HasColumnName(nameof(Entities.OrderStatusLog.Details));
        builder.Property(log => log.CreatedTime).HasColumnName(nameof(Entities.OrderStatusLog.CreatedTime));
    }
}