using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.IRepository;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public  class GetAllRestaurantsQueryHandler(ILogger<GetAllRestaurantsQueryHandler> logger, IMapper mapper, IRestaurantRepository restaurantRepository) : 
    IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDto>>
{
    public async Task<IEnumerable<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants");
        var restaurant = await restaurantRepository.GetAllMatchingAsync(request.SearchPhrase);
        var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurant);
        return restaurantsDto!;
    }
}