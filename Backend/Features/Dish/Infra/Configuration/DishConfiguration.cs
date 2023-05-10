using Backend.Features.Dish.Domain.ValueTypes;
using Backend.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Dish.Infra.Configuration;

public class DishConfiguration : IEntityTypeConfiguration<Domain.Dish>
{
    public void Configure(EntityTypeBuilder<Domain.Dish> builder)
    {
        builder.ToTable(nameof(BackendDbContext.Dishes)).HasKey(dish => dish.Id);

        builder.Property(d => d.RestaurantId).HasColumnName(nameof(Entities.Dish.RestaurantId));
        builder.Property(d => d.Price).HasColumnName(nameof(Entities.Dish.Price));
        builder.Property(d => d.Name).HasColumnName(nameof(Entities.Dish.Name));
        builder.Property(d => d.Category).HasColumnName(nameof(Entities.Dish.Category));
        builder.Property(d => d.Description).HasColumnName(nameof(Entities.Dish.Description));
        builder.Property(d => d.Photo).HasColumnName(nameof(Entities.Dish.Photo));
        builder.Property(d => d.IsVegetarian).HasColumnName(nameof(Entities.Dish.IsVegetarian));

        builder.OwnsOne(dish => dish.CachedRate, owned =>
        {
            owned.Property(rate => rate.Count).HasColumnName(Entities.DishConfiguration.CachedRateCountColumnName);
            owned.Property(rate => rate.Score)
                .HasConversion<RateScoreConverter>()
                .HasColumnName(Entities.DishConfiguration.CachedRateScoreColumnName);
        });
    }
}