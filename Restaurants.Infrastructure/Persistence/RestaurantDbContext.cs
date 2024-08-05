using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Restaurants.Domain.Entites;

namespace Restaurants.Infrastructure.Persistence;

public class RestaurantDbContext(DbContextOptions options) : DbContext(options)
{
    internal DbSet<Restaurant> Restaurants { get; set; }
    internal DbSet<Dish> Dishes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Restaurant>()
            .OwnsOne(r => r.Address);
        modelBuilder.Entity<Restaurant>()
            .HasMany(d => d.Dishes)
            .WithOne()
            .HasForeignKey(d => d.RestaurantId);
        
    }
}


public class AppDbContextFactory : IDesignTimeDbContextFactory<RestaurantDbContext>
{
    public RestaurantDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Assuming the appsettings.json file is in the same directory
            .AddJsonFile(@Directory.GetCurrentDirectory() + "/../Restaurants.API/appsettings.json")
            .Build();
        var connectionString = configuration.GetConnectionString("Default");
        var optionsBuilder = new DbContextOptionsBuilder<RestaurantDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new RestaurantDbContext(optionsBuilder.Options);
    }
}