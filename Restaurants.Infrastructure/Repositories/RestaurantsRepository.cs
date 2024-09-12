using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entites;
using Restaurants.Domain.Entities;
using Restaurants.Domain.IRepository;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class RestaurantsRepository(RestaurantDbContext context) : IRestaurantRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
    {
        var restaurants = await context.Restaurants
            .Include(r => r.Dishes)
            .ToListAsync();
        return restaurants;
    }
    
    public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageNumber, int pageSize)
    {
        var searchPhraseToLower = searchPhrase?.ToLower();
        
        var baseQuery =  context.Restaurants
            .Where(r=> searchPhraseToLower == null || 
                       (r.Name.ToLower().Contains(searchPhraseToLower) || r.Description.ToLower().Contains(searchPhraseToLower)))
            .Include(r => r.Dishes);
            
            var totalCount = await baseQuery.CountAsync();
                
            var restaurants = await baseQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
        return (restaurants, totalCount);
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

    public async Task<Guid?> CreateRestaurantAsync(Restaurant? restaurant)
    {
        if (restaurant != null)
        {
            await context.Restaurants.AddAsync(restaurant);
            await context.SaveChangesAsync();
            return restaurant.Id;
        }
        return null;
    }

    public async Task<Guid?> UpdateRestaurantAsync(Restaurant? restaurant)
{
    if (restaurant != null)
    {
        context.Restaurants.Update(restaurant);
        await context.SaveChangesAsync();
        return restaurant.Id;
    }
    return null;
}

    public async Task DeleteRestaurantAsync(Restaurant entity)
    {
        context.Restaurants.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DoesRestaurantExistByIdAsync(Guid id)
    {
        var restaurantExists = await context.Restaurants
            .AnyAsync(r => r.Id == id);
    
        return restaurantExists;
    }

    public async Task<bool> DoesRestaurantExistByNameAsync(string name)
    {
        var restaurantExists = await context.Restaurants
            .AnyAsync(r => r.Name == name);
    
        return restaurantExists;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}