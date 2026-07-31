using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.IRepository;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler (
    ILogger<UpdateRestaurantCommandHandler> logger, 
    IMapper mapper, 
    IRestaurantRepository restaurantRepository,
    IAuthorizationService authorizationService) :IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request , CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant with id: {RestaurantId} with {@UpdatedRestaurant}", request.Id, request);
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.Id, cancellationToken);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        
        if (!authorizationService.Authorise(restaurant, ResourceOperation.Update))
            throw new ForbiddenException("User does not have permission to update this restaurant");

        mapper.Map(request, restaurant);
        await restaurantRepository.UpdateRestaurantAsync(restaurant, cancellationToken);
    }
}
