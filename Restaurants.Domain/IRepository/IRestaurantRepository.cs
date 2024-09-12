using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.IRepository;

public interface IRestaurantRepository
{
    Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
    Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);
    Task<Restaurant?> GetRestaurantByIdAsync(Guid id);
    Task<Guid?> CreateRestaurantAsync (Restaurant? entity);
    Task<Guid?> UpdateRestaurantAsync (Restaurant? entity);
    Task DeleteRestaurantAsync(Restaurant entity);
    public Task<bool> DoesRestaurantExistByIdAsync(Guid id);
    public Task<bool> DoesRestaurantExistByNameAsync(string name);
    Task SaveChangesAsync();
}