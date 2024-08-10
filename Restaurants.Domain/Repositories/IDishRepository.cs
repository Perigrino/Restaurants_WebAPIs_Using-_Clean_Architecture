using Restaurants.Domain.Entites;

namespace Restaurants.Domain.Repositories;

public interface IDishRepository
{
    Task<IEnumerable<Dish>> GetAllDishesAsync();
    Task<Dish?> GetDishByIdAsync(Guid id);
    Task<Guid?> CreateDishAsync (Dish entity);
    Task<Guid?> UpdateDishAsync (Dish? entity);
    Task DeleteDishAsync(Dish entity);
    public Task<bool> DoesDishExistByIdAsync(Guid id);
    public Task<bool> DoesDishExistByNameAsync(string name);
    Task SaveChangesAsync();
}