using Backend.Infra.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infra.Data;

public class BackendDbContext : DbContext
{
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<DishInBasket> DishesInBasket { get; set; }
    public DbSet<DishRate> DishRates { get; set; }
    public DbSet<Order> Orders { get; set; }

    public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
    {
    }

    public const string OrderNumberSequenceName = $"{nameof(Orders)}_{nameof(Order.Number)}_seq";
    public const string NextOrderNumberSql = $"SELECT nextval('\"{OrderNumberSequenceName}\"')";

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(BackendDbContext).Assembly);
    }
}