using Microsoft.EntityFrameworkCore;
using Synapse.Domain.Entities;

namespace Synapse.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Note> Notes { get; set; }
    public DbSet<User> Users { get; set; }
}