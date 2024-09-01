using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entites;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.IRepository;


namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandsHandler(
    ILogger<CreateDishCommandsHandler> logger, 
    IMapper mapper, 
    IRestaurantRepository restaurantRepository, 
    IDishRepository dishRepository,
    IAuthorizationService authorizationService) : IRequestHandler<CreateDishCommand>
{
    public async Task Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new dish {@Dish}", request);
        
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.RestaurantId);
        if (restaurant is null) throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        
        if (!authorizationService.Authorise(restaurant, ResourceOperation.Delete))
            throw new ForbiddenException("User does not have permission to update this restaurant");

        var dish = mapper.Map<Dish>(request);
        await dishRepository.CreateDishAsync(dish);
    }
}