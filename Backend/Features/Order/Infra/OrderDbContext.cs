using System.Reflection;
using Backend.Features.Order.Domain.Services;
using Backend.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Order.Infra;

public class OrderDbContext : DbContext, IOrderNumberGenerator
{
    public DbSet<Domain.Dish> Dishes { get; set; } = null!;
    public DbSet<Domain.Order> Orders { get; set; } = null!;
    public DbSet<Domain.DishInBasket> DishesInBasket { get; set; } = null!;
    public DbSet<Domain.OrderStatusState> OrderStatusStates { get; set; }= null!;

    public async ValueTask<ulong> NextOrderNumber()
    {
        var dbConnection = Database.GetDbConnection();
        await dbConnection.OpenAsync();
        await using var dbCommand = dbConnection.CreateCommand();

        dbCommand.CommandText = BackendDbContext.NextOrderNumberSql;
        var result = await dbCommand.ExecuteScalarAsync();
        if (result == null)
            throw new NullReferenceException("Query returned null value");
        
        await dbConnection.CloseAsync();
        return (ulong)(long)result;
    }

    public Task<Domain.Order?> GetOrderByNumber(ulong orderNumber) =>
        Orders.Include(order => order.Dishes)
            .Include(order => order.StatusLogs)
            .AsSingleQuery()
            .FirstOrDefaultAsync(order => order.Number == orderNumber);

    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}