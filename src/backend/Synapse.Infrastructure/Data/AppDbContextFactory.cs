using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using DotNetEnv;

namespace Synapse.Infrastructure.Data;
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var root = Directory.GetParent(Directory.GetCurrentDirectory())!.FullName;
        var envPath = Path.Combine(root, ".env");
        
        Env.Load(envPath);

        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

        Console.WriteLine("DEBUG ConnectionString: " + connectionString);

        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}