using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.IRepository;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class RestaurantsRepository(RestaurantDbContext context) : IRestaurantRepository
{
    private static readonly Dictionary<string, Expression<Func<Restaurant, object>>> SortColumns = new()
    {
        { nameof(Restaurant.Name), r => r.Name },
        { nameof(Restaurant.Description), r => r.Description },
        { nameof(Restaurant.Category), r => r.Category },
    };

    public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default)
    {
        return await context.Restaurants
            .Include(r => r.Dishes)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageNumber, int pageSize, string? sortBy, SortDirection sortDirection, CancellationToken cancellationToken = default)
    {
        var searchPhraseToLower = searchPhrase?.ToLower();

        var baseQuery = context.Restaurants
            .Where(r => searchPhraseToLower == null ||
                        (r.Name.ToLower().Contains(searchPhraseToLower) || r.Description.ToLower().Contains(searchPhraseToLower)));

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        if (sortBy != null && SortColumns.TryGetValue(sortBy, out var selectedColumn))
        {
            baseQuery = sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }

        var restaurants = await baseQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (restaurants, totalCount);
    }

    public async Task<Restaurant?> GetRestaurantByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Guid> CreateRestaurantAsync(Restaurant entity, CancellationToken cancellationToken = default)
    {
        await context.Restaurants.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task UpdateRestaurantAsync(Restaurant entity, CancellationToken cancellationToken = default)
    {
        context.Restaurants.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRestaurantAsync(Restaurant entity, CancellationToken cancellationToken = default)
    {
        context.Restaurants.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DoesRestaurantExistByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Restaurants.AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<bool> DoesRestaurantExistByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await context.Restaurants.AnyAsync(r => r.Name == name, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
