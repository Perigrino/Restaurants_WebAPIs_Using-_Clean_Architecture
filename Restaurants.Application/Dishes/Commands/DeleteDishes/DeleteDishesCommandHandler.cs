using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entites;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.IRepository;
using Restaurants.Domain.Repositories;


namespace Restaurants.Application.Dishes.Commands.DeleteDishes;

public class DeleteDishesCommandHandler(
    ILogger<DeleteDishesCommandHandler> logger, 
    IRestaurantRepository restaurantRepository, 
    IDishRepository dishRepository) : 
    IRequestHandler<DeleteDishesCommand>

{
    public async Task Handle(DeleteDishesCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning("Deleting dishes from {RestaurantId}", request.RestaurantId);
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.RestaurantId);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        
        await dishRepository.DeleteDishesAsync(restaurant.Dishes);
    }
}