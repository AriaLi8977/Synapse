using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using DotNetEnv;

namespace Synapse.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var root = Directory.GetParent(Directory.GetCurrentDirectory())!.FullName;

        var envPath = Path.Combine(root, ".env");

        if (File.Exists(envPath))
        {
            Env.Load(envPath);
        }

        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        var provider =
            Environment.GetEnvironmentVariable("DatabaseProvider")
            ?? "Sqlite";

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        switch (provider.ToLower())
        {
            case "sqlite":
            {
                var connectionString =
                    Environment.GetEnvironmentVariable("ConnectionStrings__SqliteConnection")
                    ?? "Data Source=synapse.db";

                Console.WriteLine($"Using SQLite: {connectionString}");

                optionsBuilder.UseSqlite(connectionString);
                break;
            }

            case "sqlserver":
            {
                var connectionString =
                    Environment.GetEnvironmentVariable("ConnectionStrings__AzureSqlConnection");

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException(
                        "ConnectionStrings__AzureSqlConnection is missing.");
                }

                Console.WriteLine("Using SQL Server");

                optionsBuilder.UseSqlServer(connectionString);
                break;
            }

            default:
                throw new InvalidOperationException(
                    $"Unsupported DatabaseProvider: {provider}");
        }

        return new AppDbContext(optionsBuilder.Options);
    }
}