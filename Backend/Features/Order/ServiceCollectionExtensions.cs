using Backend.Features.Order.Infra;
using Common.Domain;
using Common.Infra.Dal;

namespace Backend.Features.Order;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDishFeatureServices(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Domain.OrderStatusState>>(provider =>
            new RepositoryBase<Domain.OrderStatusState>(
                provider.GetRequiredService<OrderDbContext>().OrderStatusStates));

        return services;
    }
}