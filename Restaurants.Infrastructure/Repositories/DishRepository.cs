using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entites;
using Restaurants.Domain.IRepository;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class DishRepository(RestaurantDbContext context) : IDishRepository
{
    
    public async Task<Guid?> CreateDishAsync(Dish entity)
    {
        await context.Dishes.AddAsync(entity);
        await SaveChangesAsync();
        return entity.Id;
    }

    public async Task<Dish?> GetDishByIdAsync (Guid id)
    {
        var dish = await context.Dishes.FirstOrDefaultAsync(d => d.Id == id);
        if (dish != null) 
            return dish;
        
        return null;
    }

    public async Task UpdateDishByIdAsync(Dish? entity)
    {
        context.Dishes.Update(entity);
        await SaveChangesAsync();
    }

    public async Task DeleteDishByIdAsync(Dish entity)
    {
        context.Dishes.Remove(entity);
        await SaveChangesAsync();
    }
    
    public async Task DeleteDishesAsync(IEnumerable<Dish> entity)
    {
        context.Dishes.RemoveRange(entity);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}