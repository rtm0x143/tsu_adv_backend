﻿using Microsoft.EntityFrameworkCore;

namespace Backend.Infra.Data;

public static class ConfigureDbContextExtensions
{
    /// <summary>
    /// Adds <see cref="BackendDbContext"/> to <paramref name="services"/>
    /// </summary>
    /// <returns><paramref name="services"/></returns>
    /// <exception cref="ArgumentException">When extracting connection string fron <paramref name="configuration"/> failed</exception>
    public static IServiceCollection AddBackendDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetValue<string>("BACKEND_DB_CONN")
                         ?? configuration.GetConnectionString("Default")
                         ?? throw new ArgumentException(
                             "Failed to extract connection string from configuration. Expected 'AUTH_DB_CONN' env variable or [ConnectionStrings:Default] prop in settings.");

        return services.AddDbContext<BackendDbContext>(
            configure => configure.UseNpgsql(connection));
    }
}