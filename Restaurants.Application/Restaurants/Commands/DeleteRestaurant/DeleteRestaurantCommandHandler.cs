using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entites;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.IRepository;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(
    ILogger<DeleteRestaurantCommandHandler> logger, 
    IRestaurantRepository restaurantRepository,
    IAuthorisationService authorisationService) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting restaurant with id : {RestaurantId}", request.Id);
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.Id);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        
        if (!authorisationService.Authorise(restaurant, ResourceOperation.Delete))
            throw new ForbiddenException("User does not have permission to delete this restaurant");
            
        await restaurantRepository.DeleteRestaurantAsync(restaurant);
    }
}