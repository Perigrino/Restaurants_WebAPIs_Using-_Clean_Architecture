using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.IRepository;


namespace Restaurants.Application.Dishes.Commands.UpdateDish;

public class UpdateDishCommandHandler (
    IDishRepository dishRepository, 
    IMapper mapper, 
    ILogger<UpdateDishCommandHandler> logger) : IRequestHandler<UpdateDishCommand>
{
    public async Task Handle(UpdateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating dish with id: {DishId} for restaurant {RestaurantId} with {@UpdatedDish}", request.Id, request.RestaurantId, request);
        var dish = await dishRepository.GetDishByIdAsync(request.Id, cancellationToken);
        if (dish == null)
            throw new NotFoundException(nameof(Dish), request.Id.ToString());

        if (dish.RestaurantId != request.RestaurantId)
            throw new NotFoundException(nameof(Dish), request.Id.ToString());

        mapper.Map(request, dish);
        await dishRepository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Dish with id: {DishId} has been updated successfully", request.Id);
    }

}
