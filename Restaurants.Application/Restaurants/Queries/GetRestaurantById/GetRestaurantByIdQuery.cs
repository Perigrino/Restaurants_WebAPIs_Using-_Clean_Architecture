using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQuery : IRequest<RestaurantDto?>
{
    public Guid Id { get; set; }
}