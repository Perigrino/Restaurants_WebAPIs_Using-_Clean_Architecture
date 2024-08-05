using Restaurants.Domain.Entites;

namespace Restaurants.Application.Restaurants;

public interface IRestaurantService
{
    Task<IEnumerable<Restaurant>> GetAllRestaurants();
}