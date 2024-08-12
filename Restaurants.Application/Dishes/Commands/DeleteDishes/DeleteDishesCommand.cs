using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes;

public class DeleteDishesCommand (Guid restaurantId) : IRequest
{
    public Guid RestaurantId { get; set; } = restaurantId;
}