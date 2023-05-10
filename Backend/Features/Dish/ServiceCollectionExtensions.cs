using Backend.Features.Dish.Commands;
using Backend.Features.Dish.Domain.Services;
using Backend.Features.Dish.Infra;
using Common.Domain;
using Common.Infra.Dal;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Features.Dish;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDishFeatureServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationHandler, CanRateDishAuthorizationHandler>();

        services.AddScoped<IDishRatesRepository, DishRatesRepository>()
            .AddScoped<IRepository<Domain.Dish>>(provider =>
                new RepositoryBase<Domain.Dish>(provider.GetRequiredService<DishDbContext>().Dishes))
            .AddScoped<IRepository<Domain.DishInOrder>>(provider =>
                new RepositoryBase<Domain.DishInOrder>(provider.GetRequiredService<DishDbContext>().DishesInOrder));

        return services;
    }
}