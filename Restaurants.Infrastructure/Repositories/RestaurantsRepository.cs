using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entites;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class RestaurantsRepository(RestaurantDbContext _context) : IRestaurantRepository
{
    public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync()
    {
        var restaurants = await _context.Restaurants.ToListAsync();
        return restaurants;
    }
}