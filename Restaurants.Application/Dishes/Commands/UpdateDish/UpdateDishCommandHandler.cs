using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entites;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.IRepository;
using Restaurants.Domain.Repositories;


namespace Restaurants.Application.Dishes.Commands.UpdateDish;

public class UpdateDishCommandHandler (
    IRestaurantRepository restaurantRepository, 
    IDishRepository dishRepository, 
    IMapper mapper, 
    ILogger<UpdateDishCommandHandler> logger) : IRequestHandler<UpdateDishCommand>
{
    public async Task Handle(UpdateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating dish with id: {DishId} with {@UpdatedDish}", request.Id, request);
        var dish = await dishRepository.GetDishByIdAsync(request.Id);
        if (dish == null)
            throw new NotFoundException(nameof(Dish), request.Id.ToString());

        var mappedDish = mapper.Map(request, dish);
        await dishRepository.UpdateDishByIdAsync(mappedDish);

        await restaurantRepository.SaveChangesAsync();
        logger.LogInformation("Dish with id: {DishId} has been updated successfully)", request.Id);
    }

}