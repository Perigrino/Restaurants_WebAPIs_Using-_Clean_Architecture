using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDish;

public class DeleteDishCommand (Guid restaurantId, Guid id) : IRequest
{
    public Guid DishId { get; set; } = id;
    public Guid RestaurantId { get; set; } = restaurantId;
}