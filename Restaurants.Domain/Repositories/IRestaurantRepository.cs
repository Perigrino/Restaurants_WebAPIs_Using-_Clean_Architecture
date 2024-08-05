using Restaurants.Domain.Entites;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantRepository
{
    Task<IEnumerable<Restaurant>> GetRestaurantsAsync();
}