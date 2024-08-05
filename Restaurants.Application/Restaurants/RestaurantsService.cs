using Restaurants.Domain.Entites;
using Restaurants.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantRepository restaurantRepository, ILogger<RestaurantsService> logger) : IRestaurantService
{
    public async Task<IEnumerable<Restaurant>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");
        var restaurant = await restaurantRepository.GetRestaurantsAsync();
        return restaurant;
    }
    public async Task<Restaurant?> GetAllRestaurantById(Guid id)
    {
        logger.LogInformation("Getting restaurant by id");
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(id);
        
        if (restaurant == null)
        {
            logger.LogError($"Restaurant not found with id: {id}");
            //throw new KeyNotFoundException($"Restaurant with id {id} not found.");
            return null;
            
        }
        return restaurant;
    }
}