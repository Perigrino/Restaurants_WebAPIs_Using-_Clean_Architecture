using Restaurants.Domain.Entites;

namespace Restaurants.Domain.IRepository;

public interface IDishRepository
{
    Task<Guid?> CreateDishAsync (Dish entity);
    Task<Dish?> GetDishByIdAsync(Guid id);
    Task UpdateDishByIdAsync(Dish? entity);
    Task DeleteDishByIdAsync(Dish entity);
    Task DeleteDishesAsync(IEnumerable<Dish> entity);
    Task SaveChangesAsync();
}