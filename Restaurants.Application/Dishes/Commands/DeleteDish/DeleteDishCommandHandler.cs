using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entites;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.IRepository;
using Restaurants.Domain.Repositories;
using ILogger = Serilog.ILogger;


namespace Restaurants.Application.Dishes.Commands.DeleteDish;

public class DeleteDishCommandHandler(IRestaurantRepository restaurantRepository, IDishRepository dishRepository, ILogger<DeleteDishCommandHandler> logger) : 
    IRequestHandler<DeleteDishCommand>
{
    public async Task Handle(DeleteDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning("Deleting dish with id : {DishId} from {RestaurantId}", request.DishId, request.RestaurantId);
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.RestaurantId);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.DishId.ToString());
        
        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId);
        if (dish == null) throw new NotFoundException(nameof(Dish), request.DishId.ToString());

        await dishRepository.DeleteDishByIdAsync(dish);
    }
}