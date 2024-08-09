using Restaurants.Domain.Entites;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class DishRepository(RestaurantDbContext context) : IDishRepository
{
    public async Task<IEnumerable<Dish>> GetAllDishesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Dish?> GetDishByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid?> CreateDishAsync(Dish entity)
    {
        await context.Dishes.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<Guid?> UpdateDishAsync(Dish? entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteDishAsync(Dish entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DoesDishExistByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DoesDishExistByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}