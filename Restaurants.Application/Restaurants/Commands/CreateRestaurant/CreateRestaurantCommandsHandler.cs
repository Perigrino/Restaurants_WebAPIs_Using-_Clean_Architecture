using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entites;
using Restaurants.Domain.Entities;
using Restaurants.Domain.IRepository;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandsHandler(
    ILogger<CreateRestaurantCommandsHandler> logger, 
    IMapper mapper,  
    IRestaurantRepository restaurantRepository,
    IUserContext userContext)  :IRequestHandler<CreateRestaurantCommand, Guid>
{
    public async Task<Guid> Handle(CreateRestaurantCommand request, CancellationToken token)
    {
        var currentUser = userContext.GetCurrentUser();
        
        logger.LogInformation("{UserName}: {UserID} is creating a new restaurant {@Restaurant}", currentUser.Email, currentUser.Id, request);
        
        var restaurant = mapper.Map<Restaurant>(request);
        restaurant.OwnerId = currentUser.Id;
        //Check if the restaurant exists
      
       
       var id = await restaurantRepository.CreateRestaurantAsync(restaurant);
       return (Guid)id!;
    }
}