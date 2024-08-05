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
}