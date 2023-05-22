using Backend.Infra.Data;
using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Order.Domain
{
    public partial class OrderStatusState
    {
        public const string OrderStatusLogsBackedFieldName = nameof(_orderStatusLogs);
    }
}

namespace Backend.Features.Order.Infra.Configuration
{
    public class OrderStatusStateConfiguration : IEntityTypeConfiguration<Domain.OrderStatusState>
    {
        public void Configure(EntityTypeBuilder<Domain.OrderStatusState> builder)
        {
            builder.IsDependentEntity<Domain.OrderStatusState, Entities.Order>(state => state.OrderNumber)
                .ToTable(nameof(BackendDbContext.Orders));

            builder.Property(state => state.OrderStatus).HasColumnName(nameof(Entities.Order.Status));

            builder.Navigation(order => order.OrderStatusLogs)
                .HasField(Domain.OrderStatusState.OrderStatusLogsBackedFieldName)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .AutoInclude();
        }
    }
}