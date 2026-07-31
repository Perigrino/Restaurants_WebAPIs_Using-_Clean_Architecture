using Restaurants.Domain.Entities;

namespace Restaurants.Domain.IRepository;

public interface IDishRepository
{
    Task<Guid> CreateDishAsync(Dish entity, CancellationToken cancellationToken = default);
    Task<Dish?> GetDishByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateDishByIdAsync(Dish entity, CancellationToken cancellationToken = default);
    Task DeleteDishByIdAsync(Dish entity, CancellationToken cancellationToken = default);
    Task DeleteDishesAsync(IEnumerable<Dish> entities, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
