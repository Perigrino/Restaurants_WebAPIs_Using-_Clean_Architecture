using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Entites;
using Restaurants.Domain.Entities;
using Restaurants.Domain.IRepository;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler (
    ILogger<UpdateRestaurantCommandHandler> logger, 
    IMapper mapper, 
    IRestaurantRepository restaurantRepository,
    IAuthorisationService authorisationService) :IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request , CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant with id: {RestaurantId} with {@UpdatedRestaurant}", request.Id, request);
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.Id);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        
        if (!authorisationService.Authorise(restaurant, ResourceOperation.Delete))
            throw new ForbiddenException("User does not have permission to update this restaurant");

        
        var mappedRestaurant = mapper.Map<Restaurant>(restaurant);
        await restaurantRepository.UpdateRestaurantAsync(mappedRestaurant);
        
        await restaurantRepository.SaveChangesAsync();
    }
}