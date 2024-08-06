using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entites;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandsHandler(ILogger<CreateRestaurantCommandsHandler> logger, IMapper mapper, IRestaurantRepository restaurantRepository) 
    :IRequestHandler<CreateRestaurantCommand, Guid>
{
    public async Task<Guid> Handle(CreateRestaurantCommand request, CancellationToken token)
    {
        logger.LogInformation("Creating a new restaurant {@Restaurant}", request);
        var restaurant = mapper.Map<Restaurant>(request);
        var id = await restaurantRepository.UpdateRestaurantAsync(restaurant);
        return restaurant.Id;
    }
}