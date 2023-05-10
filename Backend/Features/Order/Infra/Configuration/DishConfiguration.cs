using Backend.Infra.Data;
using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Order.Infra.Configuration;

internal class DishConfiguration : IEntityTypeConfiguration<Domain.Dish>
{
    public void Configure(EntityTypeBuilder<Domain.Dish> builder)
    {
        builder.IsDependentEntity<Domain.Dish, Entities.Dish>(d => d.Id)
            .ToTable(nameof(BackendDbContext.Dishes));

        builder.Property(d => d.RestaurantId)
            .HasColumnName(nameof(Entities.Dish.RestaurantId));
        builder.Property(d => d.Price)
            .HasColumnName(nameof(Entities.Dish.Price));
    }
}