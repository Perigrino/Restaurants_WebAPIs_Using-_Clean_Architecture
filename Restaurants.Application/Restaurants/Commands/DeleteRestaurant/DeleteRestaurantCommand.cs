using MediatR;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommand (Guid id) : IRequest
{
    public Guid Id { get; set; } = id;
}