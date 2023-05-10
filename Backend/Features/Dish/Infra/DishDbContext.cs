using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Dish.Infra;

public class DishDbContext : DbContext
{
    public DbSet<Domain.Dish> Dishes { get; set; }
    public DbSet<Domain.DishRate> DishRates { get; set; }
    public DbSet<Domain.Restaurant> Restaurants { get; set; }
    public DbSet<Domain.DishInOrder> DishesInOrder { get; set; }

    public DishDbContext(DbContextOptions<DishDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}