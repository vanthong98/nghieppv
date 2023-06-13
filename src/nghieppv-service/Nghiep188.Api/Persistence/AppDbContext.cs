using Microsoft.EntityFrameworkCore;
using Nghiep188.Api.Persistence.Entity;

namespace Nghiep188.Api.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Roll> Rolls { get; set; } = default!;
    public DbSet<ServerSeed> ServerSeeds { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}