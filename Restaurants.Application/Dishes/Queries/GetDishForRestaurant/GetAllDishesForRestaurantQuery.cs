using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetDishForRestaurant;

public class GetAllDishesForRestaurantQuery(Guid restaurantId) :IRequest<IEnumerable<DishDto>>
{
    public Guid RestaurantId { get; set; } = restaurantId;
}