//Create the Data dir and create this file

using Microsoft.EntityFrameworkCore;

namespace RedisAndEntityFrameworkInWebApi.Data;

public class KeyAndValueContext : DbContext
{
    public KeyAndValueContext(DbContextOptions<KeyAndValueContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KeyAndValue>()
            .HasKey(k => k.Key);
    }

    public DbSet<KeyAndValue>? KeyAndValues { get; set; }
}
