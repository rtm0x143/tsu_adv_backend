using Backend.Features.Dish.Infra;
using Backend.Features.Order.Infra;

namespace Backend.Infra.Data;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds
    /// <see cref="BackendDbContext"/>,
    /// <see cref="OrderDbContext"/>
    /// to <paramref name="services"/>
    /// also adds <see cref="NpgsqlDbContextOptionsFactory"/>
    /// </summary>
    /// <returns><paramref name="services"/></returns>
    /// <exception cref="ArgumentException">When extracting connection string from <paramref name="configuration"/> failed</exception>
    public static IServiceCollection AddBackendDbContexts(this IServiceCollection services,
        IConfiguration configuration)
    {
        var factory = new NpgsqlDbContextOptionsFactory(configuration);
        services.AddSingleton(factory);
        return services.AddDbContext<BackendDbContext>(factory.Construct)
            .AddDbContext<OrderDbContext>(factory.Construct)
            .AddDbContext<DishDbContext>(factory.Construct);
    }
}