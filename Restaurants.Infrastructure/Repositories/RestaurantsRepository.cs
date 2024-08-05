using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entites;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class RestaurantsRepository(RestaurantDbContext context) : IRestaurantRepository
{
    public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync()
    {
        var restaurants = await context.Restaurants.ToListAsync();
        return restaurants;
    }

    public async Task<Restaurant?> GetRestaurantByIdAsync(Guid id)
    {
        var restaurant = await context.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (restaurant != null) 
            return restaurant;
        
        return null;
    }
}