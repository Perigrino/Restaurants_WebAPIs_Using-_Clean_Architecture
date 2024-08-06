using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger, IRestaurantRepository restaurantRepository ) : 
    IRequestHandler<DeleteRestaurantCommand, bool>
{
    public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting restaurant with id : {RestaurantId}", request.Id);
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.Id);
        if (restaurant == null)
        {
            logger.LogWarning("Restaurant with id : {RestaurantId} not found", request.Id);
            return false;
        }
        await restaurantRepository.DeleteRestaurantAsync(restaurant);
        return true;
    }
}