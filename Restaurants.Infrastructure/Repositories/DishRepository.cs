using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.IRepository;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class DishRepository(RestaurantDbContext context) : IDishRepository
{
    public async Task<Guid> CreateDishAsync(Dish entity, CancellationToken cancellationToken = default)
    {
        await context.Dishes.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<Dish?> GetDishByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Dishes.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task UpdateDishByIdAsync(Dish entity, CancellationToken cancellationToken = default)
    {
        context.Dishes.Update(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteDishByIdAsync(Dish entity, CancellationToken cancellationToken = default)
    {
        context.Dishes.Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteDishesAsync(IEnumerable<Dish> entities, CancellationToken cancellationToken = default)
    {
        context.Dishes.RemoveRange(entities);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
