using Restaurants.Domain.Entites;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantRepository
{
    Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
    Task<Restaurant?> GetRestaurantByIdAsync(Guid id);
    Task<Guid?> CreateRestaurantAsync (Restaurant? entity);
    Task<Guid?> UpdateRestaurantAsync (Restaurant? entity);
    Task DeleteRestaurantAsync(Restaurant entity);
    public Task<bool> DoesRestaurantExistByIdAsync(Guid id);
    public Task<bool> DoesRestaurantExistByNameAsync(string name);
    Task SaveChangesAsync();
}