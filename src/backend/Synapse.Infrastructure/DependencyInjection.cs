using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Synapse.Infrastructure.Data;

namespace Synapse.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var provider = configuration["DatabaseProvider"];

        services.AddDbContext<AppDbContext>(options =>
        {
            if (provider == "Sqlite")
            {
                options.UseSqlite(
                    configuration.GetConnectionString("SqliteConnection"));
            }
            else
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("AzureSqlConnection"));
            }
        });

        return services;
    }
}