using Microsoft.EntityFrameworkCore;
using Abstractions.IoC;

namespace DemoApplication.Database;

public sealed class Context : DbContext
{
    public Context(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.AddConfigurationsFromAssembly();
        builder.Seed();
    }
}
