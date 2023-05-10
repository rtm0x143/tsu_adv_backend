using Common.Infra.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain = Backend.Features.Dish.Domain;

namespace Backend.Infra.Data.Entities;

internal class DishConfiguration : IEntityTypeConfiguration<Dish>
{
    public const string CachedRateCountColumnName = nameof(Dish.CachedRate) + nameof(Dish.Rate.Count);
    public const string CachedRateScoreColumnName = nameof(Dish.CachedRate) + nameof(Dish.Rate.Score);

    public void Configure(EntityTypeBuilder<Dish> builder)
    {
        builder.IsDependentEntity<Dish, Domain.Dish>(dish => dish.Id)
            .ToTable(nameof(BackendDbContext.Dishes));

        builder.Property(d => d.RestaurantId).HasColumnName(nameof(Dish.RestaurantId));
        builder.Property(d => d.Price).HasColumnName(nameof(Dish.Price));
        builder.Property(d => d.Name).HasColumnName(nameof(Dish.Name));
        builder.Property(d => d.Category).HasColumnName(nameof(Dish.Category));
        builder.Property(d => d.Description).HasColumnName(nameof(Dish.Description));
        builder.Property(d => d.Photo).HasColumnName(nameof(Dish.Photo));
        builder.Property(d => d.IsVegetarian).HasColumnName(nameof(Dish.IsVegetarian));

        builder.OwnsOne(dish => dish.CachedRate, owned =>
        {
            owned.Property(rate => rate.Count).HasColumnName(CachedRateCountColumnName);
            owned.Property(rate => rate.Score).HasColumnName(CachedRateScoreColumnName);
        });
    }
}