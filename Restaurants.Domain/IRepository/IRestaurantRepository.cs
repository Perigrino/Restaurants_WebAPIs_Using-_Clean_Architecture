using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.IRepository;

public interface IRestaurantRepository
{
    Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default);
    Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageNumber, int pageSize, string? sortBy, SortDirection sortDirection, CancellationToken cancellationToken = default);
    Task<Restaurant?> GetRestaurantByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateRestaurantAsync(Restaurant entity, CancellationToken cancellationToken = default);
    Task UpdateRestaurantAsync(Restaurant entity, CancellationToken cancellationToken = default);
    Task DeleteRestaurantAsync(Restaurant entity, CancellationToken cancellationToken = default);
    Task<bool> DoesRestaurantExistByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> DoesRestaurantExistByNameAsync(string name, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
