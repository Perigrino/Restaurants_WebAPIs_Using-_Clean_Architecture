using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;

namespace Restaurants.Tests.Repositories;

public class RestaurantsRepositoryTests
{
    [Fact]
    public async Task GetAllMatchingAsync_SearchPhrase_FiltersResults()
    {
        await using var context = CreateContext();
        context.Restaurants.AddRange(
            new Restaurant { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid().ToString(), Name = "KFC", Description = "Fried chicken", Category = "Fast Food" },
            new Restaurant { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid().ToString(), Name = "McDonald", Description = "Burgers", Category = "Fast Food" });
        await context.SaveChangesAsync();

        var repository = new RestaurantsRepository(context);
        var (items, totalCount) = await repository.GetAllMatchingAsync("kfc", 1, 10, null, SortDirection.Ascending);

        Assert.Single(items);
        Assert.Equal(1, totalCount);
        Assert.Equal("KFC", items.Single().Name);
    }

    [Fact]
    public async Task GetAllMatchingAsync_Pagination_ReturnsRequestedPage()
    {
        await using var context = CreateContext();
        for (var i = 0; i < 15; i++)
        {
            context.Restaurants.Add(new Restaurant { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid().ToString(), Name = $"Restaurant {i}", Description = "d", Category = "c" });
        }
        await context.SaveChangesAsync();

        var repository = new RestaurantsRepository(context);
        var (items, totalCount) = await repository.GetAllMatchingAsync(null, 2, 10, null, SortDirection.Ascending);

        Assert.Equal(5, items.Count());
        Assert.Equal(15, totalCount);
    }

    [Fact]
    public async Task GetAllMatchingAsync_UnknownSortBy_DoesNotThrow()
    {
        await using var context = CreateContext();
        context.Restaurants.Add(new Restaurant { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid().ToString(), Name = "KFC", Description = "Fried chicken", Category = "Fast Food" });
        await context.SaveChangesAsync();

        var repository = new RestaurantsRepository(context);
        var (items, _) = await repository.GetAllMatchingAsync(null, 1, 10, "UnknownColumn", SortDirection.Ascending);

        Assert.Single(items);
    }

    [Fact]
    public async Task CreateRestaurantAsync_PersistsAndReturnsId()
    {
        await using var context = CreateContext();
        var repository = new RestaurantsRepository(context);
        var restaurant = new Restaurant { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid().ToString(), Name = "KFC", Description = "Fried chicken", Category = "Fast Food" };

        var id = await repository.CreateRestaurantAsync(restaurant);

        Assert.Equal(restaurant.Id, id);
        Assert.Equal(1, await context.Restaurants.CountAsync());
    }

    private static RestaurantDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RestaurantDbContext(options);
    }
}
