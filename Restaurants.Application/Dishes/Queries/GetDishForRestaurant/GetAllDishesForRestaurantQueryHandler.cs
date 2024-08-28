using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entites;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.IRepository;


namespace Restaurants.Application.Dishes.Queries.GetDishForRestaurant;

public class GetAllDishesForRestaurantQueryHandler(ILogger<GetAllDishesForRestaurantQueryHandler> logger, IRestaurantRepository restaurantRepository, IMapper mapper) : 
    IRequestHandler<GetAllDishesForRestaurantQuery, 
    IEnumerable<DishDto>>
{
    public async Task<IEnumerable<DishDto>> Handle(GetAllDishesForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving dishes for restaurant with id: {RestaurantId}", request.RestaurantId);
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.RestaurantId);
        if (restaurant is null) throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        var result = mapper.Map<IEnumerable<DishDto>>(restaurant.Dishes);
        return result;
    }

}