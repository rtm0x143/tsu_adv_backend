using Backend.Infra.Data;
using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DishDomain = Backend.Features.Dish.Domain;
using OrderDomain = Backend.Features.Order.Domain;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Dish.Infra.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<DishDomain.Order>
{
    public void Configure(EntityTypeBuilder<DishDomain.Order> builder)
    {
        builder.IsDependentEntity<DishDomain.Order, OrderDomain.Order>(o => o.Number)
            .ToTable(nameof(BackendDbContext.Orders));

        builder.Property(o => o.Status).HasColumnName(nameof(Entities.Order.Status));
        builder.Property(o => o.UserId).HasColumnName(nameof(Entities.Order.UserId));
    }
}