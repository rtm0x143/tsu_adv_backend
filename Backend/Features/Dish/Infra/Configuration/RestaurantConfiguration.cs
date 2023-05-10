using Backend.Infra.Data;
using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Dish.Infra.Configuration;

public class RestaurantConfiguration : IEntityTypeConfiguration<Domain.Restaurant>
{
    public void Configure(EntityTypeBuilder<Domain.Restaurant> builder)
    {
        builder.IsDependentEntity<Domain.Restaurant, Entities.Restaurant>(rest => rest.Id)
            .ToTable(nameof(BackendDbContext.Restaurants));
    }
}