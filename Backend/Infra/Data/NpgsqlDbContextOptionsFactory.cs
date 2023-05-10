using Common.App.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infra.Data;

public class NpgsqlDbContextOptionsFactory
{
    private readonly IConfiguration _configuration;
    public NpgsqlDbContextOptionsFactory(IConfiguration configuration) => _configuration = configuration;

    public DbContextOptions Create()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        Construct(optionsBuilder);
        return optionsBuilder.Options;
    }

    public void Construct(DbContextOptionsBuilder builder)
    {
        builder.UseNpgsql(
            _configuration.GetRequiredString("BACKEND_DB_CONN", "ConnectionStrings:Default"));
    }
}