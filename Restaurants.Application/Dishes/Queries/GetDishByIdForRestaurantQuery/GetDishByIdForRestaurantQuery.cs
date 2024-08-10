using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurantQuery;

public class GetDishByIdForRestaurantQuery(Guid restaurantId, Guid dishId) : IRequest<DishDto>
{
    public Guid RestaurantId { get; set; } = restaurantId;
    public Guid DishId { get; set; } = dishId;
}