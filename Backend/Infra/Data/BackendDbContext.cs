﻿using Backend.Infra.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infra.Data;

public class BackendDbContext : DbContext
{
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<DishInCart> DishesInCart { get; set; }
    public DbSet<DishRate> DishRates { get; set; }
    public DbSet<Order> Orders { get; set; }

    public BackendDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(BackendDbContext).Assembly);
    }
}